using System.Windows.Forms;

namespace AlgoSolution.Tester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var resolution = Screen.PrimaryScreen.Bounds.Size;

            Width = resolution.Width;
            Height = resolution.Height;

            WindowState = FormWindowState.Normal;

            //var algorithm = new DonchianBreakoutMiddle_OF
            //{
            //    // Настройки для тестирования алгоритма
            //    IsTestingMode = true,
            //    CandleFilePath = @"d:\Lich\Storage\FORTS_M5\SPFB.Si.txt",

            //    // Идентификатор (у каждого алгоритма разный)
            //    Id = 1,

            //    // Параметры торгового алгоритма
            //    PeriodEntry = 50,
            //    PeriodExit = 130,
            //};

            //algorithm.Algorithm();

            //candlesChart1.Draw(algorithm);
            //performanceTable1.Draw(algorithm);
            //equityChart1.Draw(algorithm);
            //drawdownChart1.Draw(algorithm);
            //positionsChart1.Draw(algorithm);
        }
    }
}
