using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CI.Core.Domain.Models.Identity
{
    public class UserLogin
    {
        public Guid Id { get; set; }

        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
