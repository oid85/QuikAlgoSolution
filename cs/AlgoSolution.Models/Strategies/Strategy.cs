namespace AlgoSolution.Models.Strategies
{
    public class Strategy : IStrategy
    {
        public int StrategyId { get; set; }
        public string StrategyName { get; set; }
        public string Account { get; set; }
        public string FirmId { get; set; }
        public string ClassCode { get; set; }
        public string SecurityCode { get; set; }
        public int LotSize { get; set; }
        public double PercentMoney { get; set; }
        public double OptimalF { get; set; }
        public string CandlesDbTable { get; set; }
        public int CandlesLimit { get; set; }
        public int OrderQuantity { get; set; }
        public double OrderPrice { get; set; }
        public string SignalName { get; set; }
        public int CurrentPosition { get; set; }
        public double NetProfit { get; set; }
        public double CurrentNetProfit { get; set; }
        public int IsActive { get; set; }
                
        public void SetActive()
        {
            IsActive = 1;
        }

        public void SetNotActive()
        {
            IsActive = 0; ;
        }
    }
}
