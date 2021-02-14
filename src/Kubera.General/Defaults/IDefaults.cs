namespace Kubera.General
{
    public interface IDefaults
    {
        string Currency { get; }
        IDefaultGroupsCodes Groups { get; }
    }

    public interface IDefaultGroupsCodes
    {
        string Commodity { get; }
        string Crypto { get; }
        string Stock { get; }
    }
}
