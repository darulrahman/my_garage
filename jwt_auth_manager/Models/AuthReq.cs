using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt_auth_manager.Models
{
    public class AuthReq
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
