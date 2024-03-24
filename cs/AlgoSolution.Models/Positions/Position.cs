using System;

namespace AlgoSolution.Models.Positions
{
    public class Position : IPosition
    {
        public string ClassCode { get; set; }
        public string SecurityCode { get; set; }
        public double EntryPrice { get; set; }
        public double ExitPrice { get; set; }
        public DateTime EntryDateTime { get; set; }
        public DateTime ExitDateTime { get; set; }
        public int EntryBarNum { get; set; }
        public int ExitBarNum { get; set; }
        public bool IsActive { get; set; }
        public bool IsLong { get; set; }
        public bool IsShort { get; set; }
        public int Quantity { get; set; }
        public double Profit { get; set; }
        public double ProfitPct { get; set; }
        public double TotalProfit { get; set; }
        public double TotalProfitPct { get; set; }

        public void SetActive()
        {
            IsActive = true;
        }
        public void SetNotActive()
        {
            IsActive = false;
            IsLong = false;
            IsShort = false;
        }
        public void SetLong()
        {
            IsActive = true;
            IsLong = true;
            IsShort = false;
        }
        public void SetShort()
        {
            IsActive = true;
            IsLong = false;
            IsShort = true;
        }
    }
}
