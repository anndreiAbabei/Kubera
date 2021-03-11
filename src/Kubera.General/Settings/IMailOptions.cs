namespace Kubera.General.Settings
{
    public interface IMailOptions
    {
        string Api { get; }
        string ApiKey { get; }
        string SenderName { get; }
        string SenderEmail { get; }
    }
}
