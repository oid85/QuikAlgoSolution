using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AlgoSolution.Algorithms;
using AlgoSolution.Models;
using AlgoSolution.Models.Candles;
using AlgoSolution.Models.GraphElements;
using AlgoSolution.Models.Positions;

namespace AlgoSolution.GraphControls
{
    public partial class CandlesChart : UserControl, IDrawable
    {
        private IList<ICandle> _candles;
        private IList<IPosition> _positions;
        private IList<StopLimit> _stops;

        public CandlesChart()
        {
            InitializeComponent();
        }

        public void Draw(IAlgorithm algorithm)
        {
            var chartArea = new ChartArea("CandleArea")
            {
                CursorX = { IsUserSelectionEnabled = true, IsUserEnabled = true, LineColor = Color.White},                
                CursorY = { AxisType = AxisType.Secondary, LineColor = Color.White }
            };

            chartArea.BackColor = Color.Black;

            chart.ChartAreas.Add(chartArea);

            #region Свечи

            _candles = algorithm.Candles;

            if (_candles?.Count > 0)
            {
                var candleSeries = new Series("candleSeries")
                {
                    ChartType = SeriesChartType.Candlestick,
                    YAxisType = AxisType.Secondary,
                    YValuesPerPoint = 4
                };

                for (int i = 0; i < _candles.Count; i++)
                {
                    candleSeries.Points.AddXY(i, _candles[i].Low, _candles[i].High, _candles[i].Close, _candles[i].Open);

                    candleSeries.Points[candleSeries.Points.Count - 1].BorderWidth = 1;
                    candleSeries.Points[candleSeries.Points.Count - 1].AxisLabel =
                        _candles[i].DateTime.ToString("yyyy.MM.dd HH:mm:ss");

                    if (_candles[i].Open < _candles[i].Close) // Если свеча белая
                    {
                        candleSeries.Points[candleSeries.Points.Count - 1].Color = Color.Lime;
                        candleSeries.Points[candleSeries.Points.Count - 1].BorderColor = Color.Lime;
                        candleSeries.Points[candleSeries.Points.Count - 1].BackSecondaryColor = Color.Black;
                    }
                    else
                    {
                        candleSeries.Points[candleSeries.Points.Count - 1].Color = Color.Red;
                        candleSeries.Points[candleSeries.Points.Count - 1].BorderColor = Color.Red;
                        candleSeries.Points[candleSeries.Points.Count - 1].BackSecondaryColor = Color.Black;
                    }
                }

                chart.Series.Add(candleSeries);
            }

            #endregion

            #region Позиции

            _positions = algorithm.Positions;

            if (_positions?.Count > 0)
            {
                var positionSeries = new Series("positionSeries")
                {
                    ChartType = SeriesChartType.Point,
                    YAxisType = AxisType.Secondary
                };

                var openLongPositionslevels = new List<double>();
                var openShortPositionslevels = new List<double>();
                var closeLongPositionslevels = new List<double>();
                var closeShortPositionslevels = new List<double>();
                var quantities = new List<int>();

                for (int i = 0; i < _candles.Count; i++)
                {
                    openLongPositionslevels.Add(0.0);
                    openShortPositionslevels.Add(0.0);
                    closeLongPositionslevels.Add(0.0);
                    closeShortPositionslevels.Add(0.0);
                    quantities.Add(0);
                }

                for (int i = 0; i < _positions.Count; i++)
                {
                    var position = _positions[i];

                    if (position.IsLong)
                    {
                        openLongPositionslevels[position.EntryBarNum] = position.EntryPrice;
                        quantities[position.EntryBarNum] = Math.Abs(position.Quantity);

                        if (!position.IsActive)
                        {
                            closeLongPositionslevels[position.ExitBarNum] = position.ExitPrice;
                            quantities[position.ExitBarNum] = -1 * Math.Abs(position.Quantity);
                        }
                    }

                    else if (position.IsShort)
                    {
                        openShortPositionslevels[position.EntryBarNum] = position.EntryPrice;
                        quantities[position.EntryBarNum] = -1 * Math.Abs(position.Quantity);

                        if (!position.IsActive)
                        {
                            closeShortPositionslevels[position.ExitBarNum] = position.ExitPrice;
                            quantities[position.ExitBarNum] = Math.Abs(position.Quantity);
                        }
                    }
                }

                for (int i = 0; i < _candles.Count; i++)
                {
                    // Метки открытия длинных позиций
                    positionSeries.Points.AddXY(i, openLongPositionslevels[i]);

                    if (openLongPositionslevels[i] > 0.0)
                    {
                        positionSeries.Points.AddXY(i, openLongPositionslevels[i]);
                        positionSeries.Points[positionSeries.Points.Count - 1].MarkerImage = @"images\arrowGreen.png";
                        positionSeries.Points[positionSeries.Points.Count - 1].Label = $"{openLongPositionslevels[i].ToString("N0")};{quantities[i]}";
                        positionSeries.Points[positionSeries.Points.Count - 1].LabelForeColor = Color.White;                        
                    }

                    // Метки открытия коротких позиций
                    positionSeries.Points.AddXY(i, openShortPositionslevels[i]);

                    if (openShortPositionslevels[i] > 0.0)
                    {
                        positionSeries.Points.AddXY(i, openShortPositionslevels[i]);
                        positionSeries.Points[positionSeries.Points.Count - 1].MarkerImage = @"images\arrowRed.png";
                        positionSeries.Points[positionSeries.Points.Count - 1].Label = $"{openShortPositionslevels[i].ToString("N0")};{quantities[i]}";
                        positionSeries.Points[positionSeries.Points.Count - 1].LabelForeColor = Color.White;
                    }

                    // Метки закрытия длинных позиций
                    positionSeries.Points.AddXY(i, closeLongPositionslevels[i]);

                    if (closeLongPositionslevels[i] > 0.0)
                    {
                        positionSeries.Points.AddXY(i, closeLongPositionslevels[i]);
                        positionSeries.Points[positionSeries.Points.Count - 1].MarkerImage = @"images\crossGreen.png";
                        positionSeries.Points[positionSeries.Points.Count - 1].Label = $"{closeLongPositionslevels[i].ToString("N0")};{quantities[i]}";
                        positionSeries.Points[positionSeries.Points.Count - 1].LabelForeColor = Color.White;
                    }

                    // Метки закрытия коротких позиций   
                    positionSeries.Points.AddXY(i, closeShortPositionslevels[i]);

                    if (closeShortPositionslevels[i] > 0.0)
                    {
                        positionSeries.Points.AddXY(i, closeShortPositionslevels[i]);
                        positionSeries.Points[positionSeries.Points.Count - 1].MarkerImage = @"images\crossRed.png";
                        positionSeries.Points[positionSeries.Points.Count - 1].Label = $"{closeShortPositionslevels[i].ToString("N0")};{quantities[i]}";
                        positionSeries.Points[positionSeries.Points.Count - 1].LabelForeColor = Color.White;
                    }
                }

                chart.Series.Add(positionSeries);
            }

            #endregion

            #region Стопы

            _stops = algorithm.StopLimits;

            if (_stops?.Count > 0)
            {
                var stopSeries = new Series("stopSeries")
                {
                    ChartType = SeriesChartType.Point,
                    YAxisType = AxisType.Secondary
                };

                for (int i = 0; i < _candles.Count; i++)
                {
                    if (_stops[i] != null)
                    {
                        stopSeries.Points.AddXY(i, _stops[i].StopPrice);
                        stopSeries.Points[stopSeries.Points.Count - 1].MarkerStyle = MarkerStyle.Circle;
                        stopSeries.Points[stopSeries.Points.Count - 1].MarkerSize = 3;
                        stopSeries.Points[stopSeries.Points.Count - 1].MarkerColor = Color.Brown;
                    }
                    else
                    {
                        stopSeries.Points.AddXY(i, 0.0);
                        stopSeries.Points[stopSeries.Points.Count - 1].MarkerSize = 0;
                    }
                }

                chart.Series.Add(stopSeries);
            }

            #endregion

            foreach (IGraphSeries series in algorithm.GraphSeries)
            {
                var s = new Series(series.Name)
                {
                    ChartType = SeriesChartType.Line,                    
                    YAxisType = AxisType.Secondary
                };

                for (int i = 0; i < series.Values.Count; i++)
                {
                    s.Points.AddXY(i, series.Values[i]);
                    s.Points[s.Points.Count - 1].Color = series.Color;
                }

                chart.Series.Add(s);
            }

            foreach (var a in chart.ChartAreas[0].Axes)
            {
                a.MajorGrid.Enabled = false;
                a.MinorGrid.Enabled = false;
            }

            ChartResize();

            chart.CursorPositionChanged += chart_CursorPositionChanged;            
            chart.AxisViewChanged += chart_AxisViewChanged;
            chart.AxisScrollBarClicked += chart_AxisScrollBarClicked;
        }

        private void ChartResize()
        {
            var candleSeries = chart.Series.FindByName("candleSeries");
            var candleArea = chart.ChartAreas.FindByName("CandleArea");

            int startPosition = 0; // первая отображаемая свеча
            int endPosition = candleSeries.Points.Count; // последняя отображаемая свеча

            if (chart.ChartAreas[0].AxisX.ScrollBar.IsVisible)
            {
                // если уже выбран какой-то диапазон, назначаем первую и последнюю исходя из этого диапазона
                startPosition = Convert.ToInt32(candleArea.AxisX.ScaleView.Position);
                endPosition = Convert.ToInt32(candleArea.AxisX.ScaleView.Position) +
                              Convert.ToInt32(candleArea.AxisX.ScaleView.Size);
            }

            candleArea.AxisY2.Minimum = GetMinValueOnChart(_candles, startPosition, endPosition);
            candleArea.AxisY2.Maximum = GetMaxValueOnChart(_candles, startPosition, endPosition);
            
            chart.Refresh();
        }

        private double GetMinValueOnChart(IList<ICandle> candles, int start, int end)
        {
            double result = double.MaxValue;

            for (int i = start; i < end && i < candles.Count; i++)
                if (candles[i].Low < result)
                    result = candles[i].Low;

            return result;
        }

        private double GetMaxValueOnChart(IList<ICandle> candles, int start, int end)
        {
            double result = double.MinValue;

            for (int i = start; i < end && i < candles.Count; i++)
                if (candles[i].High > result)
                    result = candles[i].High;

            return result;
        }

        private void chart_CursorPositionChanged(object sender, CursorEventArgs e)
        {
            ChartResize();
        }

        private void chart_AxisViewChanged(object sender, ViewEventArgs e)
        {
            ChartResize();
        }

        private void chart_AxisScrollBarClicked(object sender, ScrollBarEventArgs e)
        {
            ChartResize();
        }
    }
}
