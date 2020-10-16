using log4net;
using log4net.Appender;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Utilities.Events;

namespace SAPConnector.ViewModels
{
    public class ProcessingStatusViewModel : BindableBase
    {
        private StringBuilder _MessageBuilder;
        public StringBuilder MessageBuilder
        {
            get { return this._MessageBuilder; }
            set
            {
                SetProperty(ref _MessageBuilder, value);
            }
        }

        public string Message
        {
            get { return this._MessageBuilder.ToString(); }
        }

        private readonly IEventAggregator _eventAgg;
        private readonly ILog _logger;

        public ProcessingStatusViewModel(IEventAggregator eventAgg, ILog logger)
        {
            _logger = logger;
            _eventAgg = eventAgg;
            MessageBuilder = new StringBuilder();
            _eventAgg.GetEvent<StatusMessageEvent>().Subscribe(UpdateMessage);
        }

        private void UpdateMessage(string obj)
        {
            _logger.Info(obj);
            MessageBuilder.AppendLine(obj);
            RaisePropertyChanged("Message");
        }

        private DelegateCommand _OpenLogFile;
        public DelegateCommand OpenLogFile =>
            _OpenLogFile ?? (_OpenLogFile = new DelegateCommand(ExecuteOpenLogFile));

        void ExecuteOpenLogFile()
        {
            try
            {
                var rootAppender = LogManager.GetRepository()
                                                      .GetAppenders()
                                                      .OfType<FileAppender>()
                                                      .FirstOrDefault(fa => fa.Name == "file");
                string PathToAttach = rootAppender != null ? rootAppender.File : string.Empty;
                if (!string.IsNullOrWhiteSpace(PathToAttach))
                    Process.Start(PathToAttach);
                else
                    UpdateMessage("Unable to read log file");
            }
            catch (Exception ex)
            {
                UpdateMessage($"Unable to read log file: {Utilities.Helper.getAllExceptionMessages(ex)}");
            }
        }
    }
}
