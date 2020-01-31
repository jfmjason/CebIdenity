﻿namespace CI.Core.Domain.Models.IdentityServerConfiguration
{
    public class ClientScope
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
