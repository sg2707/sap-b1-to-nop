using log4net;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SAPData.Models;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utilities;
using Utilities.Events;

namespace SyncToNopCommerce.ViewModels
{
    public class SettingsVViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAgg;
        public readonly ILog _logger;
        private readonly ISettingService _settings;

        public SettingsVViewModel(IEventAggregator eventAgg, ILog logger, ISettingService settings)
        {

            try
            {
                _eventAgg = eventAgg;
                _settings = settings;

                //SaveInfoCommand = new DelegateCommand(async () => await ExecuteUpdate());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _eventAgg.GetEvent<StatusMessageEvent>().Publish($"Error in Settings initialize. { Helper.getAllExceptionMessages(ex)}. Check log for more details");
            }
        }

    }
}
