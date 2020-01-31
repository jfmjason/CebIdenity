using System;
using System.Collections.Generic;

namespace CI.Core.Domain.Models.Identity
{
    public class UserClaim
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual User User { get; set; }
    }
}
