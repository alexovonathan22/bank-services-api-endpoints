using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankServices.Api.Models
{
    public class CstInfo
    {
        public string CstFName { get; set; }
        public string CstLName { get; set; }
        public string CstEmail { get; set; }
        public string CstAcctType { get; set; }
        public string Balance { get; set; }


    }
    public class CstSignIn
    {
        public string CstEmail { get; set; }
        public string CstToken { get; set; }
    }
}
