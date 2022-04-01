using SingleApi.Infrastructure.Enums;

namespace SingleApi.Data.Contracts.Models
{
    public class GatewayPspConfiguration
    {
        public int Id { get; set; }
        public int GatewayPspId { get; set; }
        public string BankId { get; set; }
        public string ConfigurationName { get; set; }
        public string Value { get; set; }
        public Channel? ChannelTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
