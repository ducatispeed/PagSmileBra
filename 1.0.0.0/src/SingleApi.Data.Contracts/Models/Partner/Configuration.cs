using SingleApi.Infrastructure.Enums;

namespace SingleApi.Data.Contracts.Models.Partner
{
    public class Configuration
    {
        public string BankId { get; set; }
        public string ConfigurationName { get; set; }
        public string Value { get; set; }
        public Channel? Channel { get; set; }
    }
}
