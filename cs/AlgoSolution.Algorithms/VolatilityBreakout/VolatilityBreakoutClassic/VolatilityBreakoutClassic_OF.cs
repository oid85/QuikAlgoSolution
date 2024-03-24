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

namespace AlgoSolution.Algorithms.VolatilityBreakout.VolatilityBreakoutClassic
{
    public class VolatilityBreakoutClassic_OF : AlgorithmBase
    {
        // Объявление и инициализация параметров торговой системы
        public int PeriodAtr { get; set; }
        public int PeriodPc { get; set; }
        public double KoeffAtrEntry { get; set; }

        public override void Execute()
        {
            int firstValidValue = 0;

            // Управление капиталом
            int lots = 1; // Количество лотов (не бумаг !!!)

            // Индикаторы
            // ATR
            IList<double> atr = new Atr(Candles, PeriodAtr).Values;
            firstValidValue = Math.Max(firstValidValue, PeriodAtr * 3);

            // Расчетные цены, от которых будет откладывать волатильность
            IList<double>  price = OpenPrices.Add(ClosePrices).DivConst(2.0);

            // Границы каналов волатильности
            IList<double> up = atr.MultConst(KoeffAtrEntry).Add(price);   // up = price + atr * KoeffAtrEntry;
            IList<double> down = atr.MultConst(KoeffAtrEntry).Add(price); // down = price + atr * KoeffAtrEntry;

            up = new Highest(up, PeriodPc).Values;
            down = new Lowest(down, PeriodPc).Values;

            up = up.Shift(1);
            down = down.Shift(1);

            firstValidValue = Math.Max(firstValidValue, PeriodPc * 2);

            // Сглаживание
            int smoothPeriod = 5;
            up = new Ema(up, smoothPeriod).Values;
            down = new Ema(down, smoothPeriod).Values;
            firstValidValue += smoothPeriod * 2;

            double startTrailing = 0.0;   // Стоп, выставляемый при открытии позиции
            double currentTrailing = 0.0; // Величина текущего стопа

            bool signalBuy = false;
            bool signalShort = false;
            double orderPrice = 0.0;

            for (int bar = firstValidValue; bar < Candles.Count - 1; bar++)
            {
                signalBuy = Candles[bar].Close > up[bar];
                signalShort = Candles[bar].Close < down[bar];

                orderPrice = Candles[bar].Close;

                double money = IsTestingMode ? Money + CurrentProfit : Money;

                MoneyManagement = new OptimalF(money, OptimalF, orderPrice, LotSize);

                if (LastActivePosition == null) // Если позиции нет
                {
                    if (signalBuy) // При получении сигнала на вход в длинную позицию
                    {
                        startTrailing = down[bar];

                        lots = MoneyManagement.PositionSize;

                        BuyAtPrice(lots, orderPrice, bar + 1);
                    }
                    else if (signalShort) // При получении сигнала на вход в короткую позицию
                    {
                        startTrailing = up[bar];

                        lots = MoneyManagement.PositionSize;

                        SellAtPrice(lots, orderPrice, bar + 1);
                    }
                }
                else // Если позиция есть
                {
                    if (LastActivePosition.IsLong)
                    {
                        int entryBar = LastActivePosition.EntryBarNum;

                        // Вычисление текущего стопа
                        currentTrailing = bar == entryBar ? startTrailing : Math.Max(currentTrailing, down[bar]);

                        // Выход по стопу
                        CloseAtStop(LastActivePosition, currentTrailing, bar + 1);
                    }

                    else if (LastActivePosition.IsShort)
                    {
                        int entryBar = LastActivePosition.EntryBarNum;

                        // Вычисление текущего стопа
                        currentTrailing = bar == entryBar ? startTrailing : Math.Min(currentTrailing, up[bar]);

                        // Выход по стопу
                        CloseAtStop(LastActivePosition, currentTrailing, bar + 1);
                    }
                }
            }

            if (IsTestingMode)
            {
                GraphSeries.Add(new GraphSeries() { Name = "up", Values = up, Color = Color.Blue });
                GraphSeries.Add(new GraphSeries() { Name = "down", Values = down, Color = Color.Red });
            }            
        }

        public VolatilityBreakoutClassic_OF(ITextFileRepositoryFactory textFileRepositoryFactory, IDataBaseRepositoryFactory dataBaseRepositoryFactory) : base(textFileRepositoryFactory, dataBaseRepositoryFactory)
        {
        }
    }
}
