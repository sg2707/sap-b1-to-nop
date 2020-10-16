using log4net;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities;
using Utilities.Events;

namespace SAPConnector.ViewModels
{
    public class AppStatusViewModel : BindableBase
    {

        private StringBuilder _Status;
        public StringBuilder Status
        {
            get { return this._Status; }
            set { SetProperty(ref _Status, value); }
        }
        private readonly IEventAggregator _eventAgg;
        public readonly ILog _logger;
        public AppStatusViewModel(ILog logger, IEventAggregator eventAgg, ISettingService settings)
        {
            _eventAgg = eventAgg;
            _logger = logger;
            Status = new StringBuilder();
            try
            {
                SqlConnectionStringBuilder con = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SAP"].ConnectionString);
                Status.Append($"SAP Info :({con.DataSource}/{con.InitialCatalog})");

            }
            catch (Exception ex)
            {
                _eventAgg.GetEvent<StatusMessageEvent>().Publish($"Error in AppStatusViewModel initialize. { Helper.getAllExceptionMessages(ex)}. Check log for more details");
                _logger.Error(ex);
            }

        }

        private DelegateCommand _ShowInfo;
        public DelegateCommand ShowInfo =>
            _ShowInfo ?? (_ShowInfo = new DelegateCommand(ExecuteShowInfo));

        void ExecuteShowInfo()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine($"App version: { Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            info.AppendLine($"Log4Net version: { AssemblyInfo.Version }");
            info.AppendLine($"Icon Credits: Eucalyp (https://www.flaticon.com/authors/eucalyp)");

            _eventAgg.GetEvent<StatusMessageEvent>().Publish(info.ToString());

        }
    }
}
