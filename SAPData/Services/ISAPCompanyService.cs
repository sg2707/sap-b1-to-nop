//using SAPbobsCOM;
using SAPData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Services
{
    //THIS SERVICE SHOULD BE ONLY CONSUMED FROM THE VIEW MODEL LEVEL
    public interface ISAPCompanyService : IDisposable
    {
        //List<CompanyInfos> MyCompanies { get; set; }
        SIUtilities.Utility utility { get; set; }

        //Task<string> ConnectCompanyAsync();
    }
}
