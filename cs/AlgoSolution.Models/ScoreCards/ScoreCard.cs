﻿using System;
using System.Collections.Generic;
using System.Linq;
using AlgoSolution.Models.Positions;
using AlgoSolution.Models.Trades;

namespace AlgoSolution.Models.ScoreCards
{
    public class ScoreCard : IScoreCard
    {
        private List<ITrade> _trades;

        public List<IPosition> Positions
        {
            get
            {
                var positions = new List<IPosition>();

                if (_trades == null || _trades.Count == 0)
                    return positions;

                var groupedTrades = new List<List<ITrade>>();
                int currentPosSize = 0;

                for (int i = 0; i < _trades.Count; i++)
                {
                    // Разбиваем каждую сделку на атомарную (атомарная сделка - сделка объемом 1)
                    var atomarTrades = GetAtomarTrades(_trades[i]);

                    for (int j = 0; j < atomarTrades.Count; j++)
                    {
                        if (currentPosSize == 0)
                            groupedTrades.Add(new List<ITrade>());

                        groupedTrades.Last().Add(atomarTrades[j]);
                        currentPosSize = groupedTrades.Last().Select(t => t.Quantity).Sum();
                    }
                }

                // Формируем позиции
                for (int i = 0; i < groupedTrades.Count; i++)
                {
                    int entryBarNum = groupedTrades[i].First().CandleIndex;
                    int exitBarNum = groupedTrades[i].Last().CandleIndex;

                    bool isLong = groupedTrades[i].First().Quantity > 0;
                    bool isShort = groupedTrades[i].First().Quantity < 0;

                    bool isActive = groupedTrades[i].Select(t => t.Quantity).Sum() != 0;

                    List<ITrade> buyTrades = groupedTrades[i].Where(v => v.Quantity > 0).OrderBy(t => t.DateTime).ToList();
                    List<ITrade> sellTrades = groupedTrades[i].Where(v => v.Quantity < 0).OrderBy(t => t.DateTime).ToList();

                    DateTime entryDateTime = DateTime.MinValue;
                    DateTime exitDateTime = DateTime.MinValue;
                    double entryPrice = 0.0;
                    double exitPrice = 0.0;
                    int quantity = 0;
                    double profit = 0.0;
                    double profitPct = 0.0;

                    if (isLong)
                    {
                        entryDateTime = buyTrades.Last().DateTime;
                        quantity = Math.Abs(buyTrades.Select(trade => trade.Quantity).Sum());
                        entryPrice = Math.Abs(buyTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;

                        if (isActive)
                        {
                            exitDateTime = DateTime.MinValue;
                            exitPrice = 0.0;
                            profit = 0.0;
                            profitPct = 0.0;
                        }
                        else
                        {
                            exitDateTime = sellTrades.Last().DateTime;
                            exitPrice = Math.Abs(sellTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;
                            profit = (exitPrice - entryPrice) * quantity;
                            profitPct = profit / quantity / entryPrice * 100.0;
                        }
                    }
                    else if (isShort)
                    {
                        entryDateTime = sellTrades.Last().DateTime;
                        quantity = Math.Abs(sellTrades.Select(trade => trade.Quantity).Sum());
                        entryPrice = Math.Abs(sellTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;

                        if (isActive)
                        {
                            exitDateTime = DateTime.MinValue;
                            exitPrice = 0.0;
                            profit = 0.0;
                            profitPct = 0.0;
                        }
                        else
                        {
                            exitDateTime = buyTrades.Last().DateTime;
                            exitPrice = Math.Abs(buyTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;
                            profit = (entryPrice - exitPrice) * quantity;
                            profitPct = profit / quantity / exitPrice * 100.0;
                        }
                    }

                    positions.Add(new Position
                    {
                        EntryBarNum = entryBarNum,
                        ExitBarNum = exitBarNum,
                        EntryPrice = entryPrice,
                        ExitPrice = exitPrice,
                        Profit = profit,
                        ProfitPct = profitPct,
                        EntryDateTime = entryDateTime,
                        ExitDateTime = exitDateTime,
                        IsActive = isActive,
                        IsLong = isLong,
                        IsShort = isShort,
                        Quantity = isLong ? quantity : -1 * quantity
                    });
                }

                positions[0].TotalProfit = positions[0].Profit;
                positions[0].TotalProfitPct = positions[0].ProfitPct;

                // Расчет общей прибыли
                for (int i = 1; i < positions.Count; i++)
                {
                    positions[i].TotalProfit = positions[i - 1].TotalProfit + positions[i].Profit;
                    positions[i].TotalProfitPct = positions[i - 1].TotalProfitPct + positions[i].ProfitPct;
                }

                return positions;
            }
        }

        public List<Tuple<DateTime, double>> EqiutyCurve
        {
            get
            {
                if (Positions == null)
                    return null;

                if (Positions.Count == 0)
                    return null;

                if (Positions.Count == 1 && Positions.First().IsActive)
                    return null;

                if (Positions.Count == 1 && !Positions.First().IsActive)
                    return null;

                var eqiutyCurve = new List<Tuple<DateTime, double>>();

                eqiutyCurve.Add(new Tuple<DateTime, double>(Positions[0].ExitDateTime, Positions[0].Profit));

                for (int i = 1; i < Positions.Count; i++)
                    if (!Positions[i].IsActive)
                        eqiutyCurve.Add(new Tuple<DateTime, double>(Positions[i].ExitDateTime, Positions[i - 1].Profit + Positions[i].Profit));

                return eqiutyCurve;
            }
        }

        public List<Tuple<DateTime, double>> DrawdownCurve
        {
            get
            {
                if (Positions == null)
                    return null;

                if (Positions.Count == 0)
                    return null;

                if (Positions.Count == 1 && Positions.First().IsActive)
                    return null;

                if (Positions.Count == 1 && !Positions.First().IsActive)
                    return null;

                var drawdownCurve = new List<Tuple<DateTime, double>>();

                double currentDrawdown = Positions[0].Profit > 0.0
                    ? 0.0
                    : Positions[0].Profit;

                drawdownCurve.Add(new Tuple<DateTime, double>(Positions[0].ExitDateTime, currentDrawdown));

                for (int i = 1; i < Positions.Count; i++)
                    if (!Positions[i].IsActive)
                    {
                        currentDrawdown = Positions[i - 1].Profit + Positions[i].Profit > 0.0
                            ? 0.0
                            : Positions[i - 1].Profit + Positions[i].Profit;

                        drawdownCurve.Add(new Tuple<DateTime, double>(Positions[i].ExitDateTime, currentDrawdown));
                    }

                return drawdownCurve;
            }
        }

        public double ProfitFactor
        {
            get
            {
                double grossProfit = Positions.Where(p => p.Profit > 0.0).Sum(p => p.Profit);
                double grossLoss = Positions.Where(p => p.Profit < 0.0).Sum(p => p.Profit);

                return grossProfit / Math.Abs(grossLoss);
            }
        }

        public double RecoveryFactor => NetProfit / MaxDrawdown;

        public double NetProfit => Positions.Last().TotalProfit;

        public double AverageProfit => Positions.Select(p => p.Profit).Average();

        public double AveragePercent => Positions.Select(p => p.ProfitPct).Average();

        public double Drawdown => DrawdownCurve.Last().Item2;

        public double MaxDrawdown => DrawdownCurve.Select(d => d.Item2).Min();

        public double MaxDrawdownPercent => Math.Abs(MaxDrawdown) / Math.Abs(NetProfit) * 100.0;

        public int NumberOfTrades => Positions.Count;

        public int WinningTrades => Positions.Count(p => p.Profit > 0.0);

        public double WinningTradesPercent => (double) WinningTrades / (double) NumberOfTrades * 100.0;

        public double StartMoney
        {
            get
            {
                if (EqiutyCurve == null)
                    return 0.0;

                if (EqiutyCurve.Count == 0)
                    return 0.0;

                return EqiutyCurve.First().Item2;
            }
        }

        public double EndMoney
        {
            get
            {
                if (EqiutyCurve == null)
                    return 0.0;

                if (EqiutyCurve.Count == 0)
                    return 0.0;

                return EqiutyCurve.Last().Item2;
            }
        }

        private List<ITrade> GetAtomarTrades(ITrade trade)
        {
            var result = new List<ITrade>();

            for (int i = 0; i < Math.Abs(trade.Quantity); i++)
            {
                var atomarTrade = new Trade
                {
                    CandleIndex = trade.CandleIndex,
                    Price = trade.Price,
                    Quantity = trade.Quantity > 0 ? 1 : -1,
                    DateTime = trade.DateTime
                };

                result.Add(atomarTrade);
            }

            return result;
        }

        public void AddTrade(ITrade trade)
        {
            if (_trades == null)
                _trades = new List<ITrade>();

            _trades.Add(trade);
            _trades = _trades.OrderBy(t => t.DateTime).ToList();
        }

        public void SetTrades(List<ITrade> trades)
        {
            _trades = trades.OrderBy(t => t.DateTime).ToList();
        }
    }
}
