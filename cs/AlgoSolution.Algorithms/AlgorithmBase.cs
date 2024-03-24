using System;
using System.Collections.Generic;
using System.Linq;
using AlgoSolution.DataAccessLayer.DataBase.Repositories;
using AlgoSolution.DataAccessLayer.TextFile.Repositories;
using AlgoSolution.Models;
using AlgoSolution.Models.Candles;
using AlgoSolution.Models.GraphElements;
using AlgoSolution.Models.MoneyManagements;
using AlgoSolution.Models.Positions;
using AlgoSolution.Models.ScoreCards;
using AlgoSolution.Models.Securities;
using AlgoSolution.Models.Trades;

namespace AlgoSolution.Algorithms
{
    public class AlgorithmBase : IAlgorithm
    {
        private readonly ITextFileRepositoryFactory _textFileRepositoryFactory;
        private readonly IDataBaseRepositoryFactory _dataBaseRepositoryFactory;

        private readonly AccountDataBaseRepository _accountDataBaseRepository;
        private readonly CandleDataBaseRepository _candleDataBaseRepository;
        private readonly SecurityDataBaseRepository _securityDataBaseRepository;
        private readonly StrategyDataBaseRepository _strategyDataBaseRepository;

        private CandleTextFileRepository _candleTxtRepository;

        public string CandleFilePath { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string SecurityCode { get; set; }

        public bool IsActive { get; set; }

        public int CandlesLimit { get; set; }

        public double Money { get; set; }

        public double PercentMoney { get; set; }

        public int LotSize { get; set; }

        public double OptimalF { get; set; }

        public IMoneyManagement MoneyManagement { get; set; }

        public IScoreCard ScoreCard { get; set; }

        public ISecurity Security { get; set; }

        public IList<ITrade> Trades { get; set; }

        public IPosition LastActivePosition
        {
            get
            {
                if (Positions == null)
                    return null;

                if (Positions.Count == 0)
                    return null;

                return Positions.Last().IsActive ? Positions.Last() : null;
            }
        }

        public int CurrentPosition
        {
            get
            {
                if (LastActivePosition == null)
                    return 0;

                if (LastActivePosition.IsLong)
                    return LastActivePosition.Quantity;

                if (LastActivePosition.IsShort)
                    return -1 * Math.Abs(LastActivePosition.Quantity);

                return 0;
            }
        }

        public double CurrentProfit
        {
            get
            {
                if (Positions == null)
                    return 0;

                if (Positions.Count == 0)
                    return 0;

                return Positions.Last().TotalProfit;
            }
        }

        public IList<IPosition> Positions => ScoreCard.Positions;

        public IList<ICandle> Candles { get; set; }

        public IList<double> OpenPrices
        {
            get { return Candles.Select(c => c.Open).ToList(); }
        }

        public IList<double> ClosePrices
        {
            get { return Candles.Select(c => c.Close).ToList(); }
        }

        public IList<double> HighPrices
        {
            get { return Candles.Select(c => c.High).ToList(); }
        }

        public IList<double> LowPrices
        {
            get { return Candles.Select(c => c.Low).ToList(); }
        }

        public IList<StopLimit> StopLimits { get; set; }

        public IList<TakeProfit> TakeProfits { get; set; }
		
        public bool IsTestingMode { get; set; }

        public void Before()
        {
            var strategy = _strategyDataBaseRepository.ReadStrategy(Id);

            Name = strategy.StrategyName;
            CandlesLimit = strategy.CandlesLimit;
            Account = strategy.Account;
            SecurityCode = strategy.SecurityCode;
            LotSize = strategy.LotSize;
            PercentMoney = strategy.PercentMoney;
            OptimalF = strategy.OptimalF;

            if (strategy.IsActive == 1)
                IsActive = true;

            if (strategy.IsActive == 0)
                IsActive = false;

            Security = _securityDataBaseRepository.ReadSecurity(strategy.ClassCode, strategy.SecurityCode);

			if (IsTestingMode 
			&& !String.IsNullOrEmpty(CandleFilePath))
			{
                _candleTxtRepository = _textFileRepositoryFactory.CreateCandleTextFileRepository(CandleFilePath);
                Candles = _candleTxtRepository.ReadCandles(CandlesLimit).OrderBy(c => c.DateTime).ToList();
            }
			else
			{
				Candles = _candleDataBaseRepository.ReadCandles(CandlesLimit, strategy.CandlesDbTable).OrderBy(c => c.DateTime).ToList();
            }

            Money = _accountDataBaseRepository.ReadAccount(Account).Money;

            Trades.Clear();

            if (!IsActive)
            {
                if (LastActivePosition == null)
                {
                    strategy.OrderQuantity = 0;
                    strategy.OrderPrice = 0.0;
                    strategy.SignalName = "";
                }                
            }

            StopLimits = new List<StopLimit>();
            TakeProfits = new List<TakeProfit>();

            for (int i = 0; i < CandlesLimit; i++)
            {
                StopLimits.Add(null);
                TakeProfits.Add(null);
            }

            _strategyDataBaseRepository.CreateOrUpdate(strategy);
        }

        public void After()
        {
            var strategy = _strategyDataBaseRepository.ReadStrategy(Id);

            if (Positions.Count == 0)
            {
                strategy.OrderQuantity = 0;
                strategy.OrderPrice = 0.0;
                strategy.SignalName = "";
            }
            else
            {
                if (LastActivePosition != null)
                {
                    if (LastActivePosition.IsLong)
                    {
                        strategy.OrderQuantity = Math.Abs(LastActivePosition.Quantity);
                        strategy.OrderPrice = LastActivePosition.EntryPrice;
                        strategy.SignalName = "";
                    }
                    else if (LastActivePosition.IsShort)
                    {
                        strategy.OrderQuantity = -1 * Math.Abs(LastActivePosition.Quantity);
                        strategy.OrderPrice = LastActivePosition.EntryPrice;
                        strategy.SignalName = "";
                    }
                }
                else
                {
                    strategy.OrderQuantity = 0;
                    strategy.OrderPrice = Positions.Last().ExitPrice;
                    strategy.SignalName = "";
                }
            }

            strategy.CurrentPosition = CurrentPosition;
            strategy.NetProfit = ScoreCard.NetProfit;
            strategy.CurrentNetProfit = 0.0;

            if (LastActivePosition != null)
            {
                if (LastActivePosition.IsLong)
                    strategy.CurrentNetProfit = Candles.Last().Close - LastActivePosition.EntryPrice;

                else if (LastActivePosition.IsShort)
                    strategy.CurrentNetProfit = LastActivePosition.EntryPrice - Candles.Last().Close;
            }

            _strategyDataBaseRepository.CreateOrUpdate(strategy);

            Console.WriteLine($"Алгоритм (ID = {Id}) выполнен...");
        }

        public void Algorithm()
        {
            Before();
            Execute();
            After();
        }

        public virtual void Execute()
        {

        }

        public void BuyAtPrice(int quantity, double price, int currentCandleIndex)
        {
            if (!IsActive)
                return;

            var trade = new Trade
            {
                CandleIndex = currentCandleIndex,
                Quantity = Math.Abs(quantity),
                Price = price,
                DateTime = Candles[currentCandleIndex].DateTime
            };

            Trades.Add(trade);
            ScoreCard.AddTrade(trade);
        }

        public void SellAtPrice(int quantity, double price, int currentCandleIndex)
        {
            if (!IsActive)
                return;

            var trade = new Trade
            {
                CandleIndex = currentCandleIndex,
                Quantity = -1 * Math.Abs(quantity),
                Price = price,
                DateTime = Candles[currentCandleIndex].DateTime
            };

            Trades.Add(trade);
            ScoreCard.AddTrade(trade);
        }

        public void CloseAtStop(IPosition position, double stopPrice, int currentCandleIndex)
        {
            if (!IsActive)
                return;
			
            // Если позиции нет, то обнуляем стоп
            if (position == null)
            {
                StopLimits[currentCandleIndex - 1] = null;
                StopLimits[currentCandleIndex] = null;
                return;	
            }

            // Если позиция есть, а стопа еще нет, то выставляем
            if (StopLimits[currentCandleIndex] == null)
            {
                StopLimits[currentCandleIndex] = new StopLimit()
                {
                    Quantity = position.Quantity,
                    StopPrice = stopPrice
                };
            }

            // Если стоп сработал, то отправляем команды
            if (position.IsLong && Candles[currentCandleIndex].Close <= StopLimits[currentCandleIndex].StopPrice)
                SellAtPrice(StopLimits[currentCandleIndex].Quantity, StopLimits[currentCandleIndex].StopPrice, currentCandleIndex);

            // Если стоп сработал, то отправляем команды
            else if (position.IsShort && Candles[currentCandleIndex].Close >= StopLimits[currentCandleIndex].StopPrice)
                BuyAtPrice(StopLimits[currentCandleIndex].Quantity, StopLimits[currentCandleIndex].StopPrice, currentCandleIndex);
        }

        public void CloseAtPrice(IPosition position, double price, int currentCandleIndex)
        {
            // Отправляем команды, если длинная позиция
            if (position.IsLong)
                SellAtPrice(position.Quantity, price, currentCandleIndex);

            // Отправляем команды, если короткая позиция
            else if (position.IsShort)
                BuyAtPrice(position.Quantity, price, currentCandleIndex);
        }

        public IList<IGraphSeries> GraphSeries { get; set; }

        public AlgorithmBase(ITextFileRepositoryFactory textFileRepositoryFactory, 
            IDataBaseRepositoryFactory dataBaseRepositoryFactory)
        {
            _textFileRepositoryFactory = textFileRepositoryFactory;
            _dataBaseRepositoryFactory = dataBaseRepositoryFactory;

            _accountDataBaseRepository = _dataBaseRepositoryFactory.CreateAccountDataBaseRepository();
            _candleDataBaseRepository = _dataBaseRepositoryFactory.CreateCandleDataBaseRepository();
            _securityDataBaseRepository = _dataBaseRepositoryFactory.CreateSecurityDataBaseRepository();
            _strategyDataBaseRepository = _dataBaseRepositoryFactory.CreateStrategyDataBaseRepository();

            Trades = new List<ITrade>();

            ScoreCard = new ScoreCard();
			
            GraphSeries = new List<IGraphSeries>();

			IsTestingMode = false;
        }
    }
}
