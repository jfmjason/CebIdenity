using System.Collections.Generic;

namespace CI.Core.Domain.Models.IdentityServerConfiguration
{
    public class ApiScope
    {
        public ApiScope()
        {
            ApiScopeClaims = new HashSet<ApiScopeClaim>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResource ApiResource { get; set; }
        public virtual ICollection<ApiScopeClaim> ApiScopeClaims { get; set; }
    }
}
