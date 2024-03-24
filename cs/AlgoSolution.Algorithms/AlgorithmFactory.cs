using AlgoSolution.Algorithms.AdaptivePCEr.AdaptivePCErClassic;
using AlgoSolution.Algorithms.AdaptivePCEr.AdaptivePCErMiddle;
using AlgoSolution.Algorithms.DonchianBreakout.DonchianBreakoutClassic;
using AlgoSolution.Algorithms.DonchianBreakout.DonchianBreakoutMiddle;
using AlgoSolution.Algorithms.DoubleBollingerBands.DoubleBollingerBandsMiddle;
using AlgoSolution.Algorithms.VolatilityBreakout.VolatilityBreakoutClassic;
using AlgoSolution.Algorithms.VolatilityBreakout.VolatilityBreakoutMiddle;
using Unity;

namespace AlgoSolution.Algorithms
{
    public class AlgorithmFactory : IAlgorithmFactory
    {
        private readonly IUnityContainer _container;

        public AlgorithmFactory(IUnityContainer container) 
        {
            _container = container;
        }

        // AdaptivePCEr
        public IAlgorithm AdaptivePCErClassic_OF(int id, int period)
        {
            var algorithm = (AdaptivePCErClassic_OF) _container.Resolve<IAlgorithm>("AdaptivePCErClassic_OF");

            algorithm.Id = id;
            algorithm.Period = period;

            return algorithm;
        }
        public IAlgorithm AdaptivePCErMiddle_OF(int id, int period)
        {
            var algorithm = (AdaptivePCErMiddle_OF)_container.Resolve<IAlgorithm>("AdaptivePCErMiddle_OF");

            algorithm.Id = id;
            algorithm.Period = period;

            return algorithm;
        }

        // DonchianBreakout
        public IAlgorithm DonchianBreakoutClassic_OF(int id, int periodEntry, int periodExit)
        {
            var algorithm = (DonchianBreakoutClassic_OF)_container.Resolve<IAlgorithm>("DonchianBreakoutClassic_OF");

            algorithm.Id = id;
            algorithm.PeriodEntry = periodEntry;
            algorithm.PeriodExit = periodExit;

            return algorithm;
        }
        public IAlgorithm DonchianBreakoutMiddle_OF(int id, int periodEntry, int periodExit)
        {
            var algorithm = (DonchianBreakoutMiddle_OF)_container.Resolve<IAlgorithm>("DonchianBreakoutMiddle_OF");

            algorithm.Id = id;
            algorithm.PeriodEntry = periodEntry;
            algorithm.PeriodExit = periodExit;

            return algorithm;
        }

        // DoubleBollingerBands
        public IAlgorithm DoubleBollingerBandsMiddle_OF(int id, int period, double mult, double stdDev)
        {
            var algorithm = (DoubleBollingerBandsMiddle_OF)_container.Resolve<IAlgorithm>("DoubleBollingerBandsMiddle_OF");

            algorithm.Id = id;
            algorithm.Period = period;
            algorithm.Mult = mult;
            algorithm.StdDev = stdDev;

            return algorithm;
        }

        // VolatilityBreakout
        public IAlgorithm VolatilityBreakoutClassic_OF(int id, int periodAtr, int periodPc, double koeffAtrEntry)
        {
            var algorithm = (VolatilityBreakoutClassic_OF)_container.Resolve<IAlgorithm>("VolatilityBreakoutClassic_OF");

            algorithm.Id = id;
            algorithm.PeriodAtr = periodAtr;
            algorithm.PeriodPc = periodPc;
            algorithm.KoeffAtrEntry = koeffAtrEntry;

            return algorithm;
        }
        public IAlgorithm VolatilityBreakoutMiddle_OF(int id, int periodAtr, int periodPc, double koeffAtrEntry)
        {
            var algorithm = (VolatilityBreakoutMiddle_OF)_container.Resolve<IAlgorithm>("VolatilityBreakoutMiddle_OF");

            algorithm.Id = id;
            algorithm.PeriodAtr = periodAtr;
            algorithm.PeriodPc = periodPc;
            algorithm.KoeffAtrEntry = koeffAtrEntry;

            return algorithm;
        }
    }
}
