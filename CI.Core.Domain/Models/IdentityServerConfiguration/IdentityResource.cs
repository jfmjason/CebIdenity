﻿using System;
using System.Collections.Generic;

namespace CI.Core.Domain.Models.IdentityServerConfiguration
{
    public class IdentityResource
    {
        public IdentityResource()
        {
            IdentityClaims = new HashSet<IdentityClaim>();
            IdentityProperties = new HashSet<IdentityProperty>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool NonEditable { get; set; }

        public virtual ICollection<IdentityClaim> IdentityClaims { get; set; }
        public virtual ICollection<IdentityProperty> IdentityProperties { get; set; }
    }
}
