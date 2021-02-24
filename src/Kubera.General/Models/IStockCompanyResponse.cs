namespace Kubera.General.Models
{
    public interface IStockCompanyResponse
    {
        string Code { get; }
        string Name { get; }
        string Exchange { get; }
        string Currency { get; }
        string Country { get; }
    }
}
