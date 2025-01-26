using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.CustomMethod;
public class CustomPaymentSettings : ISettings
{
    public bool Enabled { get; set; }
    public bool UseSandbox { get; set; }
    public string ApiKey { get; set; }
    public string ClientSecret { get; set; }
    public TransactMode TransactMode { get; set; } = TransactMode.Authorize;
}
