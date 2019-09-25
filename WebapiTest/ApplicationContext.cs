using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebapiTest
{
    public class ApplicationContext
    {
        private static Company _oCompany = null;
        private static readonly object padlock = new object();

        private static string DistSQL { get; set; }
        //private static string SqlServer { get; set; }
        private static string Server { get; set; }
        private static string CompanyDB { get; set; }
        private static string UserName { get; set; }
        private static string Password { get; set; }
        private static string UseTrusted { get; set; }
        private static string LicenseServer { get; set; }
        private static string DbUserName { get; set; }
        private static string DbPassword { get; set; }


        //public static string ConnectionString { get { return "Data Source=" + SqlServer + ";Initial Catalog=" + CompanyDB + ";user id=" + DbUserName + ";password=" + DbPassword + ";"; } }

        private ApplicationContext()
        {

        }

        public static void Open()
        {
            if (_oCompany != null) { if (_oCompany.Connected) { return; } }

            DistSQL = System.Configuration.ConfigurationManager.AppSettings["DistributationSQL"];
            // SqlServer = System.Configuration.ConfigurationManager.AppSettings["SqlServer"];
            Server = System.Configuration.ConfigurationManager.AppSettings["Server"];
            CompanyDB = System.Configuration.ConfigurationManager.AppSettings["Database"];
            UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
            Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            UseTrusted = System.Configuration.ConfigurationManager.AppSettings["useTrusted"];
            LicenseServer = System.Configuration.ConfigurationManager.AppSettings["LicenseServer"];
            DbUserName = System.Configuration.ConfigurationManager.AppSettings["DbUserName"];
            DbPassword = System.Configuration.ConfigurationManager.AppSettings["DbPassword"];

            bool trusted = false;
            Boolean.TryParse(UseTrusted, out trusted);

            if (string.IsNullOrEmpty(DistSQL)) throw new Exception("Se necesita la distribución de la base de datos MSSQL|HANA");
            if (string.IsNullOrEmpty(Server)) throw new Exception("Se necesita el nombre o dirección del servidor SAP");
            if (string.IsNullOrEmpty(CompanyDB)) throw new Exception("Se necesita del nombre de la compania a conectar");
            if (string.IsNullOrEmpty(UserName)) throw new Exception("Se necesita del nombre del usuario para acceder la compania");
            if (string.IsNullOrEmpty(Password)) throw new Exception("Se necesita de la contraseña para acceder la compania");
            if (string.IsNullOrEmpty(DbUserName)) throw new Exception("Se necesita del usuario para acceder a la base de datos");
            if (string.IsNullOrEmpty(DbPassword)) throw new Exception("Se necesita de la contraseña para acceder la base de datos");

            _oCompany = new Company
            {
                LicenseServer = (!string.IsNullOrEmpty(LicenseServer)) ? LicenseServer : null,
                Server = Server,
                UserName = UserName,
                Password = Password,
                CompanyDB = CompanyDB,
                DbUserName = DbUserName,
                DbPassword = DbPassword,
                UseTrusted = trusted
            };

            switch (DistSQL)
            {
                case "HANA": _oCompany.DbServerType = BoDataServerTypes.dst_HANADB; break;
                case "MSSQL2008": _oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008; break;
                case "MSSQL2012": _oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012; break;
                case "MSSQL2014": _oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014; break;
                case "MSSQL2016": _oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2016; break;
                case "MSSQL2017": _oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2017; break;
            }

            if (_oCompany.Connect() != 0) throw new Exception("Error(" + _oCompany.GetLastErrorCode() + "):" + _oCompany.GetLastErrorDescription());
        }

        public static bool DisconnectService()
        {
            if (_oCompany != null)
            {
                if (_oCompany.Connected)
                {
                    _oCompany.Disconnect();
                }
                else
                {
                    return false;                   //retorna 'false' cuando la conexión se encuentra apagada
                }
                _oCompany = null;
            }
            return true;                        //retorna 'true' para indicar que la conexión ha sido apagada
        }


        public static Company Db
        {
            get
            {
                lock (padlock)
                {
                    Open();
                    return _oCompany;
                }
            }
        }
    }
}