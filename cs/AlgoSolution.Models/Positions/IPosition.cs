using System;

namespace AlgoSolution.Models.Positions
{
    /// <summary>
    /// Позиция
    /// </summary>
    public interface IPosition
    {
        string ClassCode { get; set; }
        string SecurityCode { get; set; }
        double EntryPrice { get; set; }
        double ExitPrice { get; set; }
        DateTime EntryDateTime { get; set; }
        DateTime ExitDateTime { get; set; }
        int EntryBarNum { get; set; }
        int ExitBarNum { get; set; }
        bool IsActive { get; set; }
        bool IsLong { get; set; }
        bool IsShort { get; set; }
        int Quantity { get; set; }
        double Profit { get; set; }
        double ProfitPct { get; set; }
        double TotalProfit { get; set; }
        double TotalProfitPct { get; set; }
    }
}
