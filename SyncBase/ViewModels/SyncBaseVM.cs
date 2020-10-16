using log4net;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Events;

namespace SyncBase.ViewModels
{
    public abstract class SyncBaseVM : BindableBase
    {

        private string _SyncText;
        public string SyncText
        {
            get { return this._SyncText; }
            set { SetProperty(ref _SyncText, value); }
        }

        private DateTime _LastRun;
        public DateTime LastRun
        {
            get { return this._LastRun; }
            set { SetProperty(ref _LastRun, value); }
        }

        private ProgressBinder _Progress;
        public ProgressBinder Progress
        {
            get { return this._Progress; }
            set { SetProperty(ref _Progress, value); }
        }

        private bool _Enabled;
        public bool Enabled
        {
            get { return this._Enabled; }
            set { SetProperty(ref _Enabled, value); }
        }

        public readonly IEventAggregator _eventAgg;
        public readonly ILog _logger;
        public readonly ISettingService _settings;
        public readonly INotifyService _notifyService;

        public SyncBaseVM(ILog logger, IEventAggregator eventAgg, ISettingService settings, INotifyService notifyService)
        {
            Enabled = false;
            try
            {
                _logger = logger;
                _eventAgg = eventAgg;
                _settings = settings;
                _SyncText = "Sync";
                _notifyService = notifyService;
                Progress = new ProgressBinder();
                Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _eventAgg.GetEvent<StatusMessageEvent>().Publish($"Error in ProcessingBase initialize. { Helper.getAllExceptionMessages(ex)}. Check log for more details");
            }
        }

        private DelegateCommand _SyncCancel;
        public DelegateCommand SyncCancel =>
            _SyncCancel ?? (_SyncCancel = new DelegateCommand(ExecuteSyncCancel));
        void ExecuteSyncCancel()
        {
            Progress.CancellationToken.Cancel();
        }

        abstract public DelegateCommand Sync { set; get; }
        abstract public Task ExecuteSync();


    }
}
