using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AlgoSolution.Algorithms;
using AlgoSolution.Models.Positions;

namespace AlgoSolution.GraphControls
{
    public partial class PositionsChart : UserControl, IDrawable
    {
        private IList<IPosition> _positions;

        public PositionsChart()
        {
            InitializeComponent();
        }

        public void Draw(IAlgorithm algorithm)
        {
            var chartArea = new ChartArea("PositionsArea")
            {
                CursorX = { IsUserSelectionEnabled = true, IsUserEnabled = true, LineColor = Color.White},                
                CursorY = { AxisType = AxisType.Secondary, LineColor = Color.White }
            };

            chartArea.BackColor = Color.Black;

            chart.ChartAreas.Add(chartArea);


            #region Позиции

            _positions = algorithm.Positions;

            if (_positions?.Count > 0)
            {
                var positionsSeries = new Series("positionsSeries")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.Green,                    
                    YAxisType = AxisType.Secondary
                };

                for (int i = 0; i < _positions.Count; i++)
                {
                    if (_positions[i] != null)
                    {
                        positionsSeries.Points.AddXY(i, _positions[i].Profit);                                                                        
                    }
                }

                chart.Series.Add(positionsSeries);
            }

            #endregion

            ChartResize();

            chart.CursorPositionChanged += chart_CursorPositionChanged;            
            chart.AxisViewChanged += chart_AxisViewChanged;
            chart.AxisScrollBarClicked += chart_AxisScrollBarClicked;
        }

        private void ChartResize()
        {
            var positionsSeries = chart.Series.FindByName("positionsSeries");
            var chartArea = chart.ChartAreas.FindByName("PositionsArea");

            int startPosition = 0;
            int endPosition = positionsSeries.Points.Count;

            if (chart.ChartAreas[0].AxisX.ScrollBar.IsVisible)
            {
                // если уже выбран какой-то диапазон, назначаем первую и последнюю исходя из этого диапазона
                startPosition = Convert.ToInt32(chartArea.AxisX.ScaleView.Position);
                endPosition = Convert.ToInt32(chartArea.AxisX.ScaleView.Position) +
                              Convert.ToInt32(chartArea.AxisX.ScaleView.Size);
            }

            chartArea.AxisY2.Minimum = GetMinValueOnChart(_positions, startPosition, endPosition);
            chartArea.AxisY2.Maximum = GetMaxValueOnChart(_positions, startPosition, endPosition);
            
            chart.Refresh();
        }

        private double GetMinValueOnChart(IList<IPosition> positions, int start, int end)
        {
            double result = double.MaxValue;

            for (int i = start; i < end && i < positions.Count; i++)
                if (positions[i].Profit < result)
                    result = positions[i].Profit;

            return result;
        }

        private double GetMaxValueOnChart(IList<IPosition> positions, int start, int end)
        {
            double result = double.MinValue;

            for (int i = start; i < end && i < positions.Count; i++)
                if (positions[i].Profit > result)
                    result = positions[i].Profit;

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
