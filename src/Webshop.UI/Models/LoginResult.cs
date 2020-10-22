using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.UI.Models
{
    public class LoginResult
    {
        public LoginResult() { }
        public LoginResult(string userName, bool authenticated)
        {
            UserName = userName;
            Authenticated = authenticated;
        }
        public string UserName { get; set; }
        public bool Authenticated { get; }
    }
}
