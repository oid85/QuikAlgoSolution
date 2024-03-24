using System;
using System.Collections.Generic;
using System.Drawing;
using AlgoSolution.DataAccessLayer.DataBase.Repositories;
using AlgoSolution.DataAccessLayer.TextFile.Repositories;
using AlgoSolution.Indicators;
using AlgoSolution.Indicators.Common;
using AlgoSolution.Models.GraphElements;
using AlgoSolution.Models.MoneyManagements;
using Framework.Centaur.MathExtensions;

namespace AlgoSolution.Algorithms.DonchianBreakout.DonchianBreakoutMiddle
{
    public class DonchianBreakoutMiddle_OF : AlgorithmBase
    {
        // Объявление и инициализация параметров торговой системы
        public int PeriodEntry { get; set; }
        public int PeriodExit { get; set; }

        public override void Execute()
        {
            // Объявление переменных
            int firstValidValue = 0;
            int lots = 1; // Количество лотов (не бумаг !!!)

            bool signalBuy = false;
            bool signalShort = false;

            // Определяем периоды каналов
            int periodHighEntry = PeriodEntry;
            int periodLowEntry = PeriodEntry;
            int periodHighExit = PeriodExit;
            int periodLowExit = PeriodExit;

            // Цены для построения канала
            IList<double> priceForChannelHighEntry = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);
            IList<double> priceForChannelHighExit = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);
            IList<double> priceForChannelLowEntry = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);
            IList<double> priceForChannelLowExit = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);

            // Построение каналов
            IList<double> highLevelEntry = new Highest(priceForChannelHighEntry, periodHighEntry).Values;
            IList<double> lowLevelEntry = new Lowest(priceForChannelHighExit, periodLowEntry).Values;
            IList<double> highLevelExit = new Highest(priceForChannelLowEntry, periodHighExit).Values;
            IList<double> lowLevelExit = new Lowest(priceForChannelLowExit, periodLowExit).Values;

            // Сглаживание
            int smoothPeriod = 5;
            highLevelEntry = new Ema(highLevelEntry, smoothPeriod).Values;
            lowLevelEntry = new Ema(lowLevelEntry, smoothPeriod).Values;
            highLevelExit = new Ema(highLevelExit, smoothPeriod).Values;
            lowLevelExit = new Ema(lowLevelExit, smoothPeriod).Values;

            highLevelEntry = highLevelEntry.Shift(1);
            lowLevelEntry = lowLevelEntry.Shift(1);
            highLevelExit = highLevelExit.Shift(1);
            lowLevelExit = lowLevelExit.Shift(1);

            firstValidValue = Math.Max(firstValidValue, periodHighEntry);
            firstValidValue = Math.Max(firstValidValue, periodLowEntry);
            firstValidValue = Math.Max(firstValidValue, periodHighExit);
            firstValidValue = Math.Max(firstValidValue, periodLowExit);

            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int bar = firstValidValue; bar < Candles.Count - 1; bar++)
            {
                // Правило входа
                signalBuy = ClosePrices[bar] > highLevelEntry[bar];
                signalShort = ClosePrices[bar] < lowLevelEntry[bar];

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
                    double startTrailingStop = (highLevelExit[entryBar] + lowLevelExit[entryBar]) / 2.0;
                    double curTrailingStop = (highLevelExit[bar] + lowLevelExit[bar]) / 2.0;

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
                GraphSeries.Add(new GraphSeries() { Name = "highLevelEntry", Values = highLevelEntry, Color = Color.Blue });
                GraphSeries.Add(new GraphSeries() { Name = "highLevelExit", Values = highLevelExit, Color = Color.Blue });
                GraphSeries.Add(new GraphSeries() { Name = "lowLevelEntry", Values = lowLevelEntry, Color = Color.Red });
                GraphSeries.Add(new GraphSeries() { Name = "lowLevelExit", Values = lowLevelExit, Color = Color.Red });
            }            
        }

        public DonchianBreakoutMiddle_OF(ITextFileRepositoryFactory textFileRepositoryFactory, IDataBaseRepositoryFactory dataBaseRepositoryFactory) : base(textFileRepositoryFactory, dataBaseRepositoryFactory)
        {
        }
    }
}
