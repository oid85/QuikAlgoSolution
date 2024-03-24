namespace AlgoSolution.Models.Strategies
{
    public interface IStrategy
    {
        int StrategyId { get; set; }
        string StrategyName { get; set; }
        string Account { get; set; }
        string FirmId { get; set; }
        string ClassCode { get; set; }
        string SecurityCode { get; set; }
        int LotSize { get; set; }
        double PercentMoney { get; set; }
        double OptimalF { get; set; }
        string CandlesDbTable { get; set; }        
        int CandlesLimit { get; set; }
        int OrderQuantity { get; set; }
        double OrderPrice { get; set; }
        string SignalName { get; set; }
        int CurrentPosition { get; set; }
        double NetProfit { get; set; }
        double CurrentNetProfit { get; set; }
        int IsActive { get; set; }

        void SetActive();
        void SetNotActive();
    }
}
