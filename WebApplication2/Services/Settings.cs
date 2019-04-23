using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace WebApplication2.Services
{
    public class Settings
    {
        public static string filename = "sqliteDB.db";

        public static string sqliteConnectionString = "Data source =" +
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename) + "; New=false; Foreign Keys = True";//kelias i sqlite db faila

        public static bool testing = true;

        //public static string localpath = @"C:\Users\user\source\repos\WebApplication2\WebApplication2\sqliteDB.db";
        //public static string sqliteConnectionString = "Data source = " + localpath + "; New=false; Foreign Keys = True";
        private static readonly string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        public static SigningCredentials GetCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length
            //
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            return credentials;
        }
    }
}