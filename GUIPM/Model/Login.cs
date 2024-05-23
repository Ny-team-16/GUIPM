using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIPM
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UserLogin()
        {
            // Simple mock authentication. Replace with actual authentication logic.
            if (Username == "user" && Password == "password")
            {
                return true;
            }
            return false;
        }
    }
}
