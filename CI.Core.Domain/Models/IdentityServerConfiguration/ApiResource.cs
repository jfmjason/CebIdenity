using System;
using System.Collections.Generic;

namespace CI.Core.Domain.Models.IdentityServerConfiguration
{
    public class ApiResource
    {
        public ApiResource()
        {
            ApiClaims = new HashSet<ApiClaim>();
            ApiProperties = new HashSet<ApiProperty>();
            ApiScopes = new HashSet<ApiScope>();
            ApiSecrets = new HashSet<ApiSecret>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }

        public virtual ICollection<ApiClaim> ApiClaims { get; set; }
        public virtual ICollection<ApiProperty> ApiProperties { get; set; }
        public virtual ICollection<ApiScope> ApiScopes { get; set; }
        public virtual ICollection<ApiSecret> ApiSecrets { get; set; }
    }
}
