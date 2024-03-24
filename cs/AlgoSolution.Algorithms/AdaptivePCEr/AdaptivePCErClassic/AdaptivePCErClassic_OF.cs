using System;
using System.Collections.Generic;
using System.Drawing;
using AlgoSolution.DataAccessLayer.DataBase.Repositories;
using AlgoSolution.DataAccessLayer.TextFile.Repositories;
using AlgoSolution.Indicators.Common;
using AlgoSolution.Models.GraphElements;
using AlgoSolution.Models.MoneyManagements;
using Framework.Centaur.MathExtensions;

namespace AlgoSolution.Algorithms.AdaptivePCEr.AdaptivePCErClassic
{
    public class AdaptivePCErClassic_OF : AlgorithmBase
    {
        // Объявление и инициализация параметров торговой системы
        public int Period { get; set; }

        public override void Execute()
        {
            int firstValidValue = 0;

            // Управление капиталом
            int lots = 1; // Количество лотов (не бумаг !!!)

            double orderPrice = 0.0;

            bool signalBuy = false;
            bool signalShort = false;

            // Определяем периоды каналов
            int period = Period;

            // Цены для построения канала
            IList<double> priceForChannelHighEntry = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);
            IList<double> priceForChannelHighExit = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);
            IList<double> priceForChannelLowEntry = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);
            IList<double> priceForChannelLowExit = HighPrices.Add(LowPrices).Add(ClosePrices).Add(ClosePrices).DivConst(4.0);

            // Построение каналов
            IList<double> erHighEntry = new Er(priceForChannelHighEntry, period).Values;
            IList<double> erHighExit = new Er(priceForChannelHighExit, period).Values;
            IList<double> erLowEntry = new Er(priceForChannelLowEntry, period).Values;
            IList<double> erLowExit = new Er(priceForChannelLowExit, period).Values;

            // Сглаживание
            int smoothPeriod = 5;
            erHighEntry = new Ema(erHighEntry, smoothPeriod).Values;
            erHighExit = new Ema(erHighExit, smoothPeriod).Values;
            erLowEntry = new Ema(erLowEntry, smoothPeriod).Values;
            erLowExit = new Ema(erLowExit, smoothPeriod).Values;

            firstValidValue = Math.Max(firstValidValue, Convert.ToInt32(Math.Floor(period * 1.1)));

            IList<double> highLevelEntry = new List<double>().InitValues(Candles.Count);
            IList<double> highLevelExit = new List<double>().InitValues(Candles.Count);
            IList<double> lowLevelEntry = new List<double>().InitValues(Candles.Count);
            IList<double> lowLevelExit = new List<double>().InitValues(Candles.Count);

            for (int i = 0; i < Candles.Count; i++)
            {
                int nHighEntry = period - Convert.ToInt32(Math.Floor((period - 1) * erHighEntry[i]));
                int nHighExit = period - Convert.ToInt32(Math.Floor((period - 1) * erHighExit[i]));
                int nLowEntry = period - Convert.ToInt32(Math.Floor((period - 1) * erLowEntry[i]));
                int nLowExit = period - Convert.ToInt32(Math.Floor((period - 1) * erLowExit[i]));

                double maxHighEntry = priceForChannelHighEntry[i];
                double maxHighExit = priceForChannelHighExit[i];
                double minLowEntry = priceForChannelLowEntry[i];
                double minLowExit = priceForChannelLowExit[i];

                int maxN = 0;
                maxN = Math.Max(maxN, nHighEntry);
                maxN = Math.Max(maxN, nHighExit);
                maxN = Math.Max(maxN, nLowEntry);
                maxN = Math.Max(maxN, nLowExit);

                if (i >= maxN)
                {
                    for (int j = i - nHighEntry; j < i; j++)
                        if (priceForChannelHighEntry[j] > maxHighEntry)
                            maxHighEntry = priceForChannelHighEntry[j];

                    for (int j = i - nHighExit; j < i; j++)
                        if (priceForChannelHighExit[j] > maxHighExit)
                            maxHighExit = priceForChannelHighExit[j];

                    for (int j = i - nLowEntry; j < i; j++)
                        if (priceForChannelLowEntry[j] < minLowEntry)
                            minLowEntry = priceForChannelLowEntry[j];

                    for (int j = i - nLowExit; j < i; j++)
                        if (priceForChannelLowExit[j] < minLowExit)
                            minLowExit = priceForChannelLowExit[j];
                }

                highLevelEntry[i] = maxHighEntry;
                highLevelExit[i] = maxHighExit;
                lowLevelEntry[i] = minLowEntry;
                lowLevelExit[i] = minLowExit;
            }

            // Сглаживание
            smoothPeriod = 5;
            highLevelEntry = new Ema(highLevelEntry, smoothPeriod).Values;
            lowLevelEntry = new Ema(lowLevelEntry, smoothPeriod).Values;
            highLevelExit = new Ema(highLevelExit, smoothPeriod).Values;
            lowLevelExit = new Ema(lowLevelExit, smoothPeriod).Values;

            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int bar = firstValidValue; bar < Candles.Count - 1; bar++)
            {
                // Правило входа
                signalBuy = ClosePrices[bar] > highLevelEntry[bar];
                signalShort = ClosePrices[bar] < lowLevelEntry[bar];

                // Задаем цену для заявки
                orderPrice = ClosePrices[bar];

                double money = IsTestingMode ? Money + CurrentProfit : Money;

                MoneyManagement = new OptimalF(money, OptimalF, orderPrice, LotSize);

                if (LastActivePosition == null) // Если позиции нет
                {
                    if (signalBuy) // При получении сигнала на вход в длинную позицию
                    {
                        lots = MoneyManagement.PositionSize;

                        BuyAtPrice(lots, orderPrice, bar + 1);
                    }
                    else if (signalShort) // При получении сигнала на вход в короткую позицию
                    {
                        lots = MoneyManagement.PositionSize;

                        SellAtPrice(lots, orderPrice, bar + 1);
                    }
                }
                else
                {
                    int entryBar = LastActivePosition.EntryBarNum;
                    double startTrailingStop;
                    double curTrailingStop;

                    if (LastActivePosition.IsLong)
                    {
                        startTrailingStop = lowLevelExit[entryBar];
                        curTrailingStop = lowLevelExit[bar];

                        trailingStop = bar == entryBar
                            ? startTrailingStop
                            : System.Math.Max(trailingStop, curTrailingStop);
                        CloseAtStop(LastActivePosition, trailingStop, bar + 1);
                    }
                    else if (LastActivePosition.IsShort)
                    {
                        startTrailingStop = highLevelExit[entryBar];
                        curTrailingStop = highLevelExit[bar];

                        trailingStop = bar == entryBar
                            ? startTrailingStop
                            : System.Math.Min(trailingStop, curTrailingStop);
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

        public AdaptivePCErClassic_OF(ITextFileRepositoryFactory textFileRepositoryFactory, IDataBaseRepositoryFactory dataBaseRepositoryFactory) : base(textFileRepositoryFactory, dataBaseRepositoryFactory)
        {
        }
    }
}
