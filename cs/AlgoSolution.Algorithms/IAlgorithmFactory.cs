namespace AlgoSolution.Algorithms
{
    public interface IAlgorithmFactory
    {
        // AdaptivePCEr
        IAlgorithm AdaptivePCErClassic_OF(int id, int period);
        IAlgorithm AdaptivePCErMiddle_OF(int id, int period);

        // DonchianBreakout
        IAlgorithm DonchianBreakoutClassic_OF(int id, int periodEntry, int periodExit);
        IAlgorithm DonchianBreakoutMiddle_OF(int id, int periodEntry, int periodExit);

        // DoubleBollingerBands
        IAlgorithm DoubleBollingerBandsMiddle_OF(int id, int period, double mult, double stdDev);

        // VolatilityBreakout
        IAlgorithm VolatilityBreakoutClassic_OF(int id, int periodAtr, int periodPc, double koeffAtrEntry);
        IAlgorithm VolatilityBreakoutMiddle_OF(int id, int periodAtr, int periodPc, double koeffAtrEntry);
    }
}
