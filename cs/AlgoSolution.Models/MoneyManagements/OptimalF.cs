using System;

namespace AlgoSolution.Models.MoneyManagements
{
    public class OptimalF : IMoneyManagement
    {
        private readonly double _money;
        private readonly double _optimalF;
        private readonly double _price;
        private readonly double _lotSize;

        public int PositionSize
        {
            get
            {
                double result = _money * _optimalF / _price;
                result /= _lotSize;
                result = Math.Floor(result);
                result = Math.Max(result, 1.0);

                return Convert.ToInt32(result);
            }
        }

        public OptimalF(double money, double optimalF, double price, int lotSize)
        {
            _money = money;
            _optimalF = optimalF;
            _price = price;
            _lotSize = lotSize;
        }
    }
}
