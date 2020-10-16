using log4net;
//using SAPbobsCOM;
using SAPData.Models;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData
{



    public class SAPCompanyService : ISAPCompanyService
    {
        //public Company MyCompany { get; set; }
        //public List<CompanyInfos> MyCompanies { get; set; }

        readonly ILog _logger;
        readonly IConfigSettings _configService;
        readonly IProductService _module1Service;
        private byte[] key = { 11, 23, 44, 11, 2, 2, 5, 5, 11, 14, 2, 12, 3, 12, 1, 29, 241, 14, 111, 144, 173, 53, 171, 12, 12, 12, 12, 218, 131, 236, 53, 12 };

        public SIUtilities.Utility utility { get; set; }

        public SAPCompanyService(ILog logger, IConfigSettings ConfigService, IProductService Module1Service)
        {
            _logger = logger;
            _module1Service = Module1Service;
            _configService = ConfigService;
            utility = new SIUtilities.Utility(key, logger);
        }

        //public Task<string> ConnectCompanyAsync()
        //{
        //    return Task.Run(() =>
        //    {
        //        //NOTE: THIS FUNCTION CONIDERS SINGLE SAP COMPANY.
        //        //IF MULTIPLE COMPANY THEN USE ConnectMultipleCompanyAsync

        //        _configService.ReloadSettings();
        //        string msg = "";

        //        //DB Connection
        //        SqlConnectionStringBuilder cString = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SAP"].ConnectionString);

        //        MyCompany = new SAPbobsCOM.Company();
        //        MyCompany.Server = cString.DataSource;
        //        MyCompany.DbServerType = (BoDataServerTypes)Enum.Parse(typeof(BoDataServerTypes), _configService.DbServerType);
        //        MyCompany.CompanyDB = cString.InitialCatalog;
        //        MyCompany.DbUserName = cString.UserID;
        //        MyCompany.DbPassword = cString.Password;
        //        MyCompany.LicenseServer = _configService.LicenseServer;
        //        MyCompany.UserName = _configService.SAPUserName;
        //        MyCompany.Password = _configService.SAPPassword;

        //        //Connect
        //        if (!MyCompany.Connected)
        //        {
        //            int lRetCode = MyCompany.Connect();
        //            if (lRetCode != 0)
        //            {
        //                int temp_int = 0;
        //                string temp_string = null;
        //                MyCompany.GetLastError(out temp_int, out temp_string);
        //                _logger.Error("Failed to connect. Detail: " + temp_int.ToString() + " - " + temp_string);
        //                throw new Exception(temp_int.ToString() + ": " + temp_string);
        //            }
        //            else
        //            {
        //                //SIUtilities.Utility ut = new SIUtilities.Utility(null, _logger, MyCompany);
        //            }
        //        }

        //        utility.SetSAPCompany(MyCompany);
        //        msg = "SAP Connected: " + MyCompany.UserName + ": " + MyCompany.Server + "/" + MyCompany.CompanyDB;
        //        return msg;
        //    });
        //}

        //public Task<string> ConnectMultipleCompanyAsync()
        //{
        //    return Task.Run(() =>
        //    {
        //        //NOTE: THIS FUNCTION CONIDERS Multiple SAP COMPANY.
        //        //IF SINGLE COMPANY THEN USE ConnectCompanyAsync

        //        _configService.ReloadSettings();
        //        string msg = "";

        //        //DB Connection
        //        SqlConnectionStringBuilder cString = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SAP"].ConnectionString);

        //        DataTable dt = new DataTable();
        //        using (var sqlconn = new SqlConnection(cString.ToString()))
        //        {
        //            var cmd = new SqlCommand("SI_GetCompanyList", sqlconn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            var sda = new SqlDataAdapter(cmd);
        //            sqlconn.Open();
        //            sda.Fill(dt);
        //            sqlconn.Dispose();
        //        }

        //        MyCompanies = GetCompanyList(dt);
        //        if (MyCompanies?.Count <= 0)
        //            msg += "No companies to connect, please check the settings.";
        //        else
        //        {
        //            foreach (var obj in MyCompanies)
        //            {
        //                //Connect
        //                if (!obj.SAPCompany.Connected)
        //                {
        //                    int lRetCode = obj.SAPCompany.Connect();
        //                    if (lRetCode != 0)
        //                    {
        //                        int temp_int = 0;
        //                        string temp_string = null;
        //                        obj.SAPCompany.GetLastError(out temp_int, out temp_string);

        //                        msg += "ERR: " + obj.SAPCompany.UserName + ": " + obj.SAPCompany.Server + "/" + obj.SAPCompany.CompanyDB + Environment.NewLine;
        //                        _logger.Error("Failed to connect. Detail: " + temp_int.ToString() + " - " + temp_string);
        //                    }
        //                    else
        //                    {
        //                        msg += "SAP Connected: " + obj.SAPCompany.UserName + ": " + obj.SAPCompany.Server + "/" + obj.SAPCompany.CompanyDB + Environment.NewLine;
        //                        _logger.Debug("SAP Connected: " + obj.SAPCompany.UserName + ": " + obj.SAPCompany.Server + "/" + obj.SAPCompany.CompanyDB);
        //                    }

        //                }

        //            }
        //        }



        //        MyCompanies = MyCompanies.Where(x => x.SAPCompany.Connected == true).ToList();


        //        return msg;
        //    });
        //}

        //private List<CompanyInfos> GetCompanyList(DataTable dt)
        //{
        //    List<CompanyInfos> list = new List<CompanyInfos>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        list.Add(new CompanyInfos()
        //        {
        //            SAPCompany = new Company()
        //            {
        //                Server = dt.Rows[i]["Server"].ToString(),
        //                LicenseServer = dt.Rows[i]["LicenseServer"].ToString(),
        //                CompanyDB = dt.Rows[i]["CompanyDB"].ToString(),
        //                DbUserName = dt.Rows[i]["DBUser"].ToString(),
        //                DbPassword = dt.Rows[i]["DBPass"].ToString(),
        //                DbServerType = (BoDataServerTypes)Enum.Parse(typeof(BoDataServerTypes), dt.Rows[i]["DBServerType"].ToString()),
        //                UserName = dt.Rows[i]["CompanyUser"].ToString(),
        //                Password = dt.Rows[i]["CompanyPass"].ToString()
        //            },
        //            SourcePath = dt.Rows[i]["SourcePath"].ToString(),
        //            ArchivePath = dt.Rows[i]["ArchivePath"].ToString()

        //        });

        //    }
        //    return list;
        //}


        ~SAPCompanyService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);
            //if (MyCompanies != null)
            //{
            //    foreach (var obj in MyCompanies)
            //    {
            //        if (obj.SAPCompany != null && obj.SAPCompany.Connected)
            //            obj.SAPCompany.Disconnect();
            //        utility.MemoryUtilities(obj.SAPCompany);
            //    }
            //}
        }

    }
}
