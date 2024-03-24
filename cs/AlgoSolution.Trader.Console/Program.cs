using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Threading.Timer;
using AlgoSolution.Algorithms;
using Unity;
using AlgoSolution.Models;
using AlgoSolution.DataAccessLayer.DataBase;
using AlgoSolution.DataAccessLayer.TextFile;

namespace AlgoSolution.Trader.Console
{
    class Program
    {
        private static IList<IAlgorithm> _algorithms;

        private static IAlgorithmFactory _algorithmFactory;

        private const int RefreshRate = 20 * 1000;
        private static int _count = 0;

        static Timer _operTimer;
        static bool _operTimerOn;

        static void InitStrategies()
        {
            _algorithms = new List<IAlgorithm>();

            // АЛГОРИТМ 001
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(1, 50));

            // АЛГОРИТМ 002
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(2, 50));

            // АЛГОРИТМ 003
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(3, 50));

            // АЛГОРИТМ 004
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(4, 50));

            // АЛГОРИТМ 005
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(5, 50));

            // АЛГОРИТМ 006
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(6, 50));

            // АЛГОРИТМ 007
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(7, 50));

            // АЛГОРИТМ 008
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(8, 50));

            // АЛГОРИТМ 009
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(9, 50));

            // АЛГОРИТМ 010
            _algorithms.Add(_algorithmFactory.AdaptivePCErClassic_OF(10, 50));
        }

        static void Main()
        {
            var container = new UnityContainer();

            container.AddNewExtension<AlgoSolutionAlgorithmsContainerExtentions>();
            container.AddNewExtension<AlgoSolutionDataAccessLayerDataBaseContainerExtensions>();
            container.AddNewExtension<AlgoSolutionDataAccessLayerTextFilesContainerExtensions>();
            container.AddNewExtension<AlgoSolutionModelsContainerExtensions>();

            _algorithmFactory = container.Resolve<IAlgorithmFactory>();

            _operTimerOn = true;
            _operTimer = new Timer(OperTimerTick, null, 5000, RefreshRate);

            // Ожидание нажатия любой клавиши для выхода
            System.Console.ReadKey();

            _operTimerOn = false;

            if (_operTimer != null)
            {
                _operTimer.Dispose();
                _operTimer = null;
            }
        }

        private static void OperTimerTick(object state)
        {
            if (!_operTimerOn)
                return;

            _operTimerOn = false;

            _count++;

            if (_count > 2)
            {
                _count = 1;
                System.Console.Clear();
            }

            if (DateTime.Now < DateTime.Today.AddHours(10).AddMinutes(0).AddSeconds(0))
            {
                System.Console.WriteLine($"{_count}. {DateTime.Now}. Не торговое время");
                _operTimerOn = true;
                return;
            }

            if (DateTime.Now > DateTime.Today.AddHours(23).AddMinutes(45).AddSeconds(0))
            {
                System.Console.WriteLine($"{_count}. {DateTime.Now}. Не торговое время");
                _operTimerOn = true;
                return;
            }

            InitStrategies();

            // Запуск алгоритмов
            System.Console.WriteLine($"{_count}. {DateTime.Now}");

            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < _algorithms.Count; i++)
            {
                _algorithms[i].Algorithm();
                Thread.Sleep(100);
            }

            sw.Stop();

            _algorithms.Clear();

            System.Console.WriteLine(String.Format($"{sw.Elapsed.TotalSeconds} сек."));
            System.Console.WriteLine();

            _operTimerOn = true;
        }
    }
}
