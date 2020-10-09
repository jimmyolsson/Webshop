using System.Collections.Generic;

namespace Webshop.UI.Models
{
    public class ApplicationUserModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
        public List<ApplicationClaims> Claims { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
