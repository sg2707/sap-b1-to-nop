using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using log4net;
using SyncToNopCommerce.Properties;
using SyncToNopCommerce.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using SAPConnector.Views;
using Utilities;
using Utilities.Events;
using CategoryModule;

namespace SAPConnector.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IModuleManager _moduleManager;
        private readonly IModuleCatalog _moduleCatalog;
        private readonly IEventAggregator _eventAgg;
        private readonly IRegionManager _regionManager;
        public readonly ILog _logger;

        private string _title = $"NOP Sync | {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(ILog logger,
                                    IModuleCatalog moduleCatalog,
                                    IModuleManager moduleManager,
                                    IEventAggregator eventAgg,
                                    IRegionManager regionManager)
        {
            try
            {
                _logger = logger;
                _moduleCatalog = moduleCatalog;
                _moduleManager = moduleManager;
                _eventAgg = eventAgg;
                _regionManager = regionManager;

                _regionManager.RegisterViewWithRegion(RegionNames.StatusRegion, typeof(ProcessingStatus));
                _regionManager.RegisterViewWithRegion(RegionNames.AppStatusRegion, typeof(AppStatus));
                _regionManager.RegisterViewWithRegion(RegionNames.StatusRegion, typeof(SettingsV));
                Text = "Open Settings";
                LoadModules();

                //to load settings
                LoadSettings = new DelegateCommand(async () => await ExecuteUpdate());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _eventAgg.GetEvent<StatusMessageEvent>().Publish($"Error in initialize. { Helper.getAllExceptionMessages(ex)}. Check log for more details");
            }
        }

        private void LoadModules()
        {
            _moduleManager.LoadModule(typeof(Module1.ProductModule).Name);
            _moduleManager.LoadModule(typeof(OrderModule.OrderModule).Name);
            _moduleManager.LoadModule(typeof(ManufacturerModule.ManufacturerModule).Name);
            _moduleManager.LoadModule(typeof(CategoryModule.CategoryModule).Name); 
        }

        ///setting screen binding
       
        private string _text;
        public string Text
        {
            get { return this._text; }
            set { SetProperty(ref _text, value); }
        }

        bool flag = false;
        public DelegateCommand LoadSettings { get; set; }
        public async Task ExecuteUpdate()
        {
            try
            {
                flag = !flag;
                if (flag)
                {
                    Text = "Close Settings";
                    _regionManager.RequestNavigate(RegionNames.StatusRegion, "SettingsV");
                }
                else
                {
                    Text = "Open Settings";
                    _regionManager.RequestNavigate(RegionNames.StatusRegion, "ProcessingStatus");
                }



            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _eventAgg.GetEvent<StatusMessageEvent>().Publish($"Error in settings screen: {Helper.getAllExceptionMessages(ex)}");
            }

        }


    }
}
