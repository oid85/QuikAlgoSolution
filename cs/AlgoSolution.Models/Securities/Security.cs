namespace AlgoSolution.Models.Securities
{
    public class Security : ISecurity
    {
        public string SecurityCode { get; set; }
        public string ClassCode { get; set; }
        public double Go { get; set; }
    }
}
