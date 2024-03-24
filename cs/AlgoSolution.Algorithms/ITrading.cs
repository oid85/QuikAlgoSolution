using System.Collections.Generic;
using AlgoSolution.Models;
using AlgoSolution.Models.Positions;

namespace AlgoSolution.Algorithms
{
    public interface ITrading
    {
        IList<StopLimit> StopLimits { get; set; }
        IList<TakeProfit> TakeProfits { get; set; }

        void BuyAtPrice(int quantity, double price, int currentCandleIndex);
        void SellAtPrice(int quantity, double price, int currentCandleIndex);
        void CloseAtStop(IPosition position, double stopPrice, int currentCandleIndex);
        void CloseAtPrice(IPosition position, double price, int currentCandleIndex);
    }
}
