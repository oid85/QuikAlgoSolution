using System.Windows.Forms;
using AlgoSolution.Algorithms;
using System.Drawing;

namespace AlgoSolution.GraphControls
{
    public partial class PerformanceTable : UserControl, IDrawable
    {
        public PerformanceTable()
        {
            InitializeComponent();
        }

        public void Draw(IAlgorithm algorithm)
        {
            // StartMoney
            dataGridView1.Rows.Add("StartMoney", $"{algorithm.ScoreCard.StartMoney.ToString("N2")} р.");

            // EndMoney
            dataGridView1.Rows.Add("EndMoney", $"{algorithm.ScoreCard.EndMoney.ToString("N2")} р.");

            // ProfitFactor
            dataGridView1.Rows.Add("ProfitFactor", $"{algorithm.ScoreCard.ProfitFactor.ToString("N2")}");

            // RecoveryFactor
            dataGridView1.Rows.Add("RecoveryFactor", $"{algorithm.ScoreCard.RecoveryFactor.ToString("N2")}");

            // NetProfit
            dataGridView1.Rows.Add("NetProfit", $"{algorithm.ScoreCard.NetProfit.ToString("N2")} р.");
            if (algorithm.ScoreCard.NetProfit < 0.0)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Coral;
            else if (algorithm.ScoreCard.NetProfit > 0.0)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.DodgerBlue;

            // AverageProfit
            dataGridView1.Rows.Add("AverageProfit", $"{algorithm.ScoreCard.AverageProfit.ToString("N2")} р.");
            if (algorithm.ScoreCard.AverageProfit < 0.0)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Coral;
            else if (algorithm.ScoreCard.AverageProfit > 0.0)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.DodgerBlue;

            // AveragePercent
            dataGridView1.Rows.Add("AveragePercent", $"{algorithm.ScoreCard.AveragePercent.ToString("N2")} %");
            if (algorithm.ScoreCard.AveragePercent < 0.0)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Coral;
            else if (algorithm.ScoreCard.AveragePercent > 0.0)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.DodgerBlue;

            // Drawdown
            dataGridView1.Rows.Add("Drawdown", $"{algorithm.ScoreCard.Drawdown.ToString("N2")} р.");

            // MaxDrawdown
            dataGridView1.Rows.Add("MaxDrawdown", $"{algorithm.ScoreCard.MaxDrawdown.ToString("N2")} р.");

            // MaxDrawdownPercent
            dataGridView1.Rows.Add("MaxDrawdownPercent", $"{algorithm.ScoreCard.MaxDrawdownPercent.ToString("N2")} %");

            // NumberOfTrades
            dataGridView1.Rows.Add("NumberOfTrades", $"{algorithm.ScoreCard.NumberOfTrades.ToString("N0")} шт.");

            // WinningTrades
            dataGridView1.Rows.Add("WinningTrades", $"{algorithm.ScoreCard.WinningTrades.ToString("N0")} шт.");

            // WinningTradesPercent
            dataGridView1.Rows.Add("WinningTradesPercent", $"{algorithm.ScoreCard.WinningTradesPercent.ToString("N2")} %");
        }
    }
}
