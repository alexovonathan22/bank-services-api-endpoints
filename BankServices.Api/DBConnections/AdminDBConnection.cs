using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BankServices.Api.Models;
using System.Net.Mail;
using BankServices.Api.Utility;

namespace BankServices.Api.Helpers
{
    public class AdminDBConnection
    {
        public static string AdminFname { get; set; }
        public static string AdminLname { get; set; }
        public static string AdminEmail { get; set; }
        public static string Code { get; set; }
        private static string Pwd = "api@@let";
        public static string Hashpwd { get; set; }
        public static SqlConnection con = new SqlConnection(@"Data Source=ALEX-OVO-NATHAN\SQLSERVER2017DEV;Initial Catalog=Bank__DB;Integrated Security=True");
        public static int cstId { get; set; }
        //Admin signup
        public static string AdminSignUpDB(AdminInfo adminDetails)
        {
            AdminFname = adminDetails.AdminFName;
            AdminLname = adminDetails.AdminLName;
            AdminEmail = adminDetails.AdminEmail;
            Code = PasswordCreation.GenerateCode();
            Hashpwd = HashPassword.ComputeSha256Hash(Code);
            DateTime current = DateTime.Now;

            SqlCommand cmd = new SqlCommand();

            try
            {

                if (AdminFname == "" || AdminLname == "")
                {
                    throw new Exception();
                }

                if (AdminEmail == "")
                {
                    throw new Exception();
                }

                if (!AdminEmail.Contains("@gmail.com"))
                {
                    throw new Exception();
                }
                cmd.Parameters.Clear();
                cmd.Connection = con;
                cmd.CommandText = @"INSERT INTO Admin(AdminFirstName, AdminLastName, AdminEmail, AdminToken, Created_at) VALUES(@fname, @lname, @email, @token, @timestamp)";

                cmd.Parameters.AddWithValue("@fname", AdminFname);
                cmd.Parameters.AddWithValue("@lname", AdminLname);
                cmd.Parameters.AddWithValue("@email", AdminEmail);
                cmd.Parameters.AddWithValue("@token", Hashpwd);
                cmd.Parameters.AddWithValue("@timestamp", current);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("aov.nathan@gmail.com");
                    mail.To.Add(AdminEmail);
                    mail.Subject = "Admin Info";
                    mail.Body = $"Your Access code: {Code} \n Use email and access code to login";
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("aov.nathan@gmail.com", Pwd);
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    return $"Failed from inner catch msg sending why => {ex.Message}";
                }

                return $"Sucessfull Signup";

            }
            catch (Exception ex)
            {
                return $"Failed from catch why => {ex.Message}";
            }

           
        }

        //admin signIn
        public static string SignIn(AdminSignIn loginDetails)
        {
            try
            {
                string hashpassword = HashPassword.ComputeSha256Hash(loginDetails.AdminToken);
               
                string qry = string.Format("SELECT 1 FROM Admin where AdminEmail='{0}' and AdminToken='{1}'", loginDetails.AdminEmail, hashpassword);
                //SqlConnection con = new SqlConnection(@"Data Source=ALEX-OVO-NATHAN\SQLSERVER2017DEV;Initial Catalog=Bank__DB;Integrated Security=True");

                SqlCommand cmd = new SqlCommand(qry, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    con.Close();
                    return "LOGIN SUCCESSFULLY";
                }
                else
                {
                    con.Close();
                    return "INVALID EMAIL OR PASSWORD";
                }
            }
            catch(Exception ex)
            {
                return $"Check login details =>: \n{ex.Message}";
            }
        }

        public static string CstCreateAcct(CstInfo newLoginDetails)
        {
            
            Code = PasswordCreation.GenerateCode();
            Hashpwd = HashPassword.ComputeSha256Hash(Code);

            DateTime current = DateTime.Now;
            
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmdAct = new SqlCommand();


            try
            {
                

                if (newLoginDetails.CstFName == "" || newLoginDetails.CstLName == "")
                {
                    throw new Exception();
                }

                if (newLoginDetails.CstEmail == "")
                {
                    throw new Exception();
                }

                if (!newLoginDetails.CstEmail.Contains("@gmail.com"))
                {
                    throw new Exception();
                }
                cmd.Parameters.Clear();
                cmd.Connection = con;
                cmd.CommandText = @"INSERT INTO NewCstTable(cst_FirstName, cst_LastName, cst_Email, cst_Password, Created_acct_at) OUTPUT INSERTED.id_cst VALUES(@fname, @lname, @email, @token, @timestamp)";

                cmd.Parameters.AddWithValue("@fname", newLoginDetails.CstFName);
                cmd.Parameters.AddWithValue("@lname", newLoginDetails.CstLName);
                cmd.Parameters.AddWithValue("@email", newLoginDetails.CstEmail);
                cmd.Parameters.AddWithValue("@token", Hashpwd);
                cmd.Parameters.AddWithValue("@timestamp", current);
                con.Open();
                cstId = Convert.ToInt32(cmd.ExecuteScalar());//cmd.ExecuteNonQuery();
                con.Close();

                cmdAct.Parameters.Clear();
                cmdAct.Connection = con;
                cmdAct.CommandText = string.Format("insert into AccountTable  values(@acttype, @cstid, @actnum, @bal)");
                cmdAct.Parameters.AddWithValue("@acttype", newLoginDetails.CstAcctType);
                cmdAct.Parameters.AddWithValue("@cstid", cstId);
                cmdAct.Parameters.AddWithValue("@actnum", PasswordCreation.GenerateAccount());
                cmdAct.Parameters.AddWithValue("@bal", Convert.ToDouble(newLoginDetails.Balance));

                con.Open();
                cmdAct.ExecuteNonQuery();
                
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("apitest.aov@gmail.com");
                    mail.To.Add(newLoginDetails.CstEmail);
                    mail.Subject = "Cst Info";
                    mail.Body = $"Your Access code: {Code} \nUse your email {newLoginDetails.CstEmail} and access code to login";
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("apitest.aov@gmail.com", Pwd);
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    return $"CstFailed from inner catch msg sending why => {ex.Message}";
                }

                return $"Customer sucessfull Signup => {cstId}";

            }
            catch (Exception ex)
            {
                return $"CStFailed from catch why => {ex.Message}";
            }
            finally
            {
                con.Close();
            }
        
        }
 
    }
}

