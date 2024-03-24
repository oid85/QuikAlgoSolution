namespace AlgoSolution.Models.Securities
{
    /// <summary>
    /// Инструмент
    /// </summary>
    public interface ISecurity
    {
        string SecurityCode { get; set; }
        string ClassCode { get; set; }
        double Go { get; set; }
    }
}
