using AlgoSolution.Algorithms.AdaptivePCEr.AdaptivePCErClassic;
using AlgoSolution.Algorithms.AdaptivePCEr.AdaptivePCErMiddle;
using AlgoSolution.Algorithms.DonchianBreakout.DonchianBreakoutClassic;
using AlgoSolution.Algorithms.DonchianBreakout.DonchianBreakoutMiddle;
using AlgoSolution.Algorithms.DoubleBollingerBands.DoubleBollingerBandsMiddle;
using AlgoSolution.Algorithms.VolatilityBreakout.VolatilityBreakoutClassic;
using AlgoSolution.Algorithms.VolatilityBreakout.VolatilityBreakoutMiddle;
using Unity;
using Unity.Extension;

namespace AlgoSolution.Algorithms
{
    public class AlgoSolutionAlgorithmsContainerExtentions : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IAlgorithm, AdaptivePCErClassic_OF>("AdaptivePCErClassic_OF");
            Container.RegisterType<IAlgorithm, AdaptivePCErMiddle_OF>("AdaptivePCErMiddle_OF");

            Container.RegisterType<IAlgorithm, DonchianBreakoutClassic_OF>("DonchianBreakoutClassic_OF");
            Container.RegisterType<IAlgorithm, DonchianBreakoutMiddle_OF>("DonchianBreakoutMiddle_OF");

            Container.RegisterType<IAlgorithm, DoubleBollingerBandsMiddle_OF>("DoubleBollingerBandsMiddle_OF");
            
            Container.RegisterType<IAlgorithm, VolatilityBreakoutClassic_OF>("VolatilityBreakoutClassic_OF");
            Container.RegisterType<IAlgorithm, VolatilityBreakoutMiddle_OF>("VolatilityBreakoutMiddle_OF");

            Container.RegisterType<IAlgorithmFactory, AlgorithmFactory>();
        }
    }
}
