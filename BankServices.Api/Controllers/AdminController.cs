using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServices.Api.Models;
using BankServices.Api.DBConnections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankServices.Api.Helpers;

namespace BankServices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // GET: api/AdminC:\Users\aovna\source\repos\BankServicesApi.Solution\BankServices.Api\Program.cs
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Admin/5
        [HttpGet("{id}", Name = "Getname")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Admin
        [HttpPost]
        public string Post([FromBody] AdminInfo adminInfo)
        {
           return AdminDBConnection.AdminSignUpDB(adminInfo);           
        }

        // POST: api/Admin/login
        [HttpPost("login")]
        public string Post([FromBody] AdminSignIn logindetails)
        {
            return AdminDBConnection.SignIn(logindetails);
        }

        //POST: api/Admin/createcustomer
        [HttpPost("createcustomer")]
        public string Post([FromBody] CstInfo cstInfo)
        {
            return AdminDBConnection.CstCreateAcct(cstInfo);
        }

        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
