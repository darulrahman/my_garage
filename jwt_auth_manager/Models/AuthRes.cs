using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt_auth_manager.Models
{
    public class AuthRes
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public double Expires { get; set; }
    }
}
