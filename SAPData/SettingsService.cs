using log4net;
using SAPData.Models;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Keys;

namespace SAPData
{

    public class SettingsService : ISettingService
    {
        ILog _logger;
        Type SqlServerProvider;
        public SettingsService(ILog logger)
        {
            _logger = logger;
            try
            {
                //This line is here to make compiler understand that the EntityFramework.SqlServer.dll
                //is needed for the project.
                SqlServerProvider = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

            }
            catch (Exception ex)
            {
                _logger.Error($"Error in settings initialization: {ex.Message}", ex);
            }
        }
    }
}
