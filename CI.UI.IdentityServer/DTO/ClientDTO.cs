using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CI.UI.IdentityServer.DTO
{
    public class ClientViewItemDTO
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public DateTime? LastAccessed { get; set; }
    }
}
