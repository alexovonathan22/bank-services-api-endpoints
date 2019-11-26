using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankServices.Api.Utility
{
    public static class PasswordCreation
    {
        public static string GenerateCode()
        {
            Random r = new Random();
            int genCode = r.Next(1000, 5000);
            return genCode.ToString();
        }

        public static string GenerateAccount()
        {
            Random r = new Random();
            int genCode = r.Next(1000000999, 1999999999);
            return genCode.ToString();
        }

    }
}
