namespace CI.Core.Domain.Models.IdentityServerConfiguration
{
    public class ApiScopeClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiScopeId { get; set; }

        public virtual ApiScope ApiScope { get; set; }
    }
}
