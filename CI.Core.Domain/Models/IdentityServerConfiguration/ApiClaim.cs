namespace CI.Core.Domain.Models.IdentityServerConfiguration
{
    public class ApiClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
