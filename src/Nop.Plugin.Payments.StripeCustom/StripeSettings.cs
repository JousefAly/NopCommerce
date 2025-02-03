using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.StripeCustom;
public class StripeSettings : ISettings
{
    public string SecretKey { get; set; }
    public string PublishableKey { get; set; }
    public string WebhookSecret { get; set; }
    public bool Use3DS { get; set; }
}
