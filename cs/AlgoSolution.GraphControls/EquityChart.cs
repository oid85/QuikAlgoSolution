using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AlgoSolution.Algorithms;

namespace AlgoSolution.GraphControls
{
    public partial class EquityChart : UserControl, IDrawable
    {
        private List<Tuple<DateTime, double>> _eqiutyCurve;

        public EquityChart()
        {
            InitializeComponent();
        }

        public void Draw(IAlgorithm algorithm)
        {
            var chartArea = new ChartArea("EqiutyCurveArea")
            {
                CursorX = { IsUserSelectionEnabled = true, IsUserEnabled = true, LineColor = Color.White},                
                CursorY = { AxisType = AxisType.Secondary, LineColor = Color.White }
            };

            chartArea.BackColor = Color.Black;

            chart.ChartAreas.Add(chartArea);


            #region Эквити

            _eqiutyCurve = algorithm.ScoreCard.EqiutyCurve;

            if (_eqiutyCurve?.Count > 0)
            {
                var eqiutyCurveSeries = new Series("eqiutyCurveSeries")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.Lime,                    
                    YAxisType = AxisType.Secondary
                };

                for (int i = 0; i < _eqiutyCurve.Count; i++)
                {
                    if (_eqiutyCurve[i] != null)
                    {                        
                        eqiutyCurveSeries.Points.AddXY(_eqiutyCurve[i].Item1, _eqiutyCurve[i].Item2);                                                                        
                    }
                }

                chart.Series.Add(eqiutyCurveSeries);
            }

            #endregion

            ChartResize();

            chart.CursorPositionChanged += chart_CursorPositionChanged;            
            chart.AxisViewChanged += chart_AxisViewChanged;
            chart.AxisScrollBarClicked += chart_AxisScrollBarClicked;
        }

        private void ChartResize()
        {
            var eqiutyCurveSeries = chart.Series.FindByName("eqiutyCurveSeries");
            var chartArea = chart.ChartAreas.FindByName("EqiutyCurveArea");

            int startPosition = 0;
            int endPosition = eqiutyCurveSeries.Points.Count;

            if (chart.ChartAreas[0].AxisX.ScrollBar.IsVisible)
            {
                // если уже выбран какой-то диапазон, назначаем первую и последнюю исходя из этого диапазона
                startPosition = Convert.ToInt32(chartArea.AxisX.ScaleView.Position);
                endPosition = Convert.ToInt32(chartArea.AxisX.ScaleView.Position) +
                              Convert.ToInt32(chartArea.AxisX.ScaleView.Size);
            }

            chartArea.AxisY2.Minimum = GetMinValueOnChart(_eqiutyCurve, startPosition, endPosition);
            chartArea.AxisY2.Maximum = GetMaxValueOnChart(_eqiutyCurve, startPosition, endPosition);
            
            chart.Refresh();
        }

        private double GetMinValueOnChart(List<Tuple<DateTime, double>> eqiutyCurve, int start, int end)
        {
            double result = double.MaxValue;

            for (int i = start; i < end && i < eqiutyCurve.Count; i++)
                if (eqiutyCurve[i].Item2 < result)
                    result = eqiutyCurve[i].Item2;

            return result;
        }

        private double GetMaxValueOnChart(List<Tuple<DateTime, double>> eqiutyCurve, int start, int end)
        {
            double result = double.MinValue;

            for (int i = start; i < end && i < eqiutyCurve.Count; i++)
                if (eqiutyCurve[i].Item2 > result)
                    result = eqiutyCurve[i].Item2;

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
