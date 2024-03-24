using System;
using System.Collections.Generic;
using System.Drawing;
using AlgoSolution.DataAccessLayer.DataBase.Repositories;
using AlgoSolution.DataAccessLayer.TextFile.Repositories;
using AlgoSolution.Indicators.Common;
using AlgoSolution.Models.GraphElements;
using AlgoSolution.Models.MoneyManagements;
using Framework.Centaur.MathExtensions;

namespace AlgoSolution.Algorithms.DoubleBollingerBands.DoubleBollingerBandsMiddle
{
    public class DoubleBollingerBandsMiddle_OF : AlgorithmBase
    {
        // Объявление и инициализация параметров торговой системы
        public int Period { get; set; }
        public double Mult { get; set; }
        public double StdDev { get; set; }

        public override void Execute()
        {
            // Объявление переменных
            int firstValidValue = 0;
            int lots = 1; // Количество лотов (не бумаг !!!)

            bool signalBuy = false;
            bool signalShort = false;

            int periodSmall = Period;
            double mult = Mult;
            double stdDev = StdDev;
            int periodBig = Convert.ToInt32(Math.Floor(periodSmall * mult));

            // Цены для построения канала
            IList<double> priceForChannel = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);

            // Построение каналов
            IList<double> highLevelSmall = new BBandUpper(priceForChannel, periodSmall, stdDev).Values;
            IList<double> lowLevelSmall = new BBandLower(priceForChannel, periodSmall, stdDev).Values;
            IList<double> highLevelBig = new BBandUpper(priceForChannel, periodBig, stdDev).Values;
            IList<double> lowLevelBig = new BBandLower(priceForChannel, periodBig, stdDev).Values;

            firstValidValue = Math.Max(firstValidValue, periodSmall);
            firstValidValue = Math.Max(firstValidValue, periodBig);

            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int bar = firstValidValue; bar < Candles.Count - 1; bar++)
            {
                // Правило входа
                signalBuy = ClosePrices[bar] > highLevelSmall[bar];
                signalBuy &= ClosePrices[bar] > (highLevelBig[bar] + lowLevelBig[bar]) / 2.0;

                signalShort = ClosePrices[bar] < lowLevelSmall[bar];
                signalShort &= ClosePrices[bar] < (highLevelBig[bar] + lowLevelBig[bar]) / 2.0;

                // Задаем цену для заявки
                double orderPrice = Candles[bar].Close;

                double money = IsTestingMode ? Money + CurrentProfit : Money;

                MoneyManagement = new OptimalF(money, OptimalF, orderPrice, LotSize);

                if (LastActivePosition == null)
                {
                    if (signalBuy)
                    {
                        lots = MoneyManagement.PositionSize;
                        BuyAtPrice(lots, orderPrice, bar + 1);
                    }
                    else if (signalShort)
                    {
                        lots = MoneyManagement.PositionSize;
                        SellAtPrice(lots, orderPrice, bar + 1);
                    }
                }
                else
                {
                    int entryBar = LastActivePosition.EntryBarNum;
                    double startTrailingStop = (highLevelSmall[entryBar] + lowLevelSmall[entryBar]) / 2.0;
                    double curTrailingStop = (highLevelSmall[bar] + lowLevelSmall[bar]) / 2.0;

                    if (LastActivePosition.IsLong)
                    {
                        trailingStop = bar == entryBar
                            ? startTrailingStop
                            : Math.Max(trailingStop, curTrailingStop);

                        // Выход по стопу
                        CloseAtStop(LastActivePosition, trailingStop, bar + 1);
                    }

                    else if (LastActivePosition.IsShort)
                    {
                        trailingStop = bar == entryBar
                            ? startTrailingStop
                            : Math.Min(trailingStop, curTrailingStop);

                        // Выход по стопу
                        CloseAtStop(LastActivePosition, trailingStop, bar + 1);
                    }
                }
            }

            if (IsTestingMode)
            {
                GraphSeries.Add(new GraphSeries() { Name = "highLevelSmall", Values = highLevelSmall, Color = Color.Blue });
                GraphSeries.Add(new GraphSeries() { Name = "lowLevelSmall", Values = lowLevelSmall, Color = Color.Blue });
                GraphSeries.Add(new GraphSeries() { Name = "highLevelBig", Values = highLevelBig, Color = Color.Red });
                GraphSeries.Add(new GraphSeries() { Name = "lowLevelBig", Values = lowLevelBig, Color = Color.Red });
            }            
        }

        public DoubleBollingerBandsMiddle_OF(ITextFileRepositoryFactory textFileRepositoryFactory, IDataBaseRepositoryFactory dataBaseRepositoryFactory) : base(textFileRepositoryFactory, dataBaseRepositoryFactory)
        {
        }
    }
}
