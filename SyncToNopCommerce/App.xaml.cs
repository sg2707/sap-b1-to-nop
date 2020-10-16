using SAPConnector.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using System;
using System.Configuration;
using System.Reflection;
using System.Linq;
using Utilities;
using log4net;
using SAPData;
using SAPData.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Module1.ViewModels;

namespace SAPConnector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        List<StartupParams> paramList = new List<StartupParams>();
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }


        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            var _Logger = LogManager.GetLogger("Logger");
            containerRegistry.RegisterInstance(_Logger);

            ConfigSettingsService _ConfigService = new ConfigSettingsService(_Logger);
            containerRegistry.RegisterInstance<IConfigSettings>(_ConfigService);

            NotifyService _NotifyService = new NotifyService(_Logger, _ConfigService);
            containerRegistry.RegisterInstance<INotifyService>(_NotifyService);

            //This should not be instance, as we need this as seperate instance for each process.
            containerRegistry.Register<ISAPCompanyService, SAPCompanyService>();

            SettingsService _SettingsService = new SettingsService(_Logger);
            containerRegistry.RegisterInstance<ISettingService>(_SettingsService);

        }


        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            if (paramList?.Count == 0 || paramList.Contains(StartupParams.MODULE1))
            {
                var ExchRateModuleType = typeof(Module1.ProductModule);
                moduleCatalog.AddModule(new ModuleInfo()
                {
                    ModuleName = ExchRateModuleType.Name,
                    ModuleType = ExchRateModuleType.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.OnDemand
                });
            }

        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            foreach (var item in e.Args)
            {
                try
                {
                    var _param = (StartupParams)Enum.Parse(typeof(StartupParams), item.ToUpper());
                    paramList.Add(_param);
                }
                catch (Exception ex)
                {
                    MainWindow.Visibility = Visibility.Visible;
                    paramList.Clear();
                    Application.Current.Shutdown();
                    break;
                }
            }

            base.OnStartup(e);

            // Auto run the scheduled process here and exit the program.
            if (paramList.Count > 0)
            {
                var _tasks = new List<Task>();
                StringBuilder Errors = new StringBuilder();
                SyncBase.ViewModels.SyncBaseVM SyncVM = null;
                foreach (var item in paramList)
                {
                    switch (item)
                    {
                        case StartupParams.MODULE1:
                            SyncVM = Container.Resolve<ProductSyncViewModel>();
                            break;
                    }

                    if (SyncVM.Enabled)
                        _tasks.Add(SyncVM.ExecuteSync());
                    else
                        Errors.AppendLine($"Error in intializing {item} module.");
                }

                await Task.WhenAll(_tasks.ToArray());
                SyncVM._logger.Info("---------------- End of Auto run ---------------");
                if (Errors.Length > 0)
                {
                    await SyncVM._notifyService.SendNotification($"Error occured in Schedule. Details: { Errors.ToString()}");
                }

                Application.Current.Shutdown();
            }

        }

        enum StartupParams
        {
            MODULE1
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }
    }
}
