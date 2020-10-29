
using CategoryModule.ViewModels;
using log4net;
using NopAPIConnect;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SAPData;
using SAPData.Services;
using SyncBase.Views;

namespace CategoryModule
{
    public class CategoryModule : IModule
    {
        private IContainerProvider _containerProvider;
        private readonly ILog _logger;
        private readonly IConfigSettings _configService;
        public CategoryModule(ILog logger, IConfigSettings configService)
        {
            _logger = logger;
            _configService = configService;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(Utilities.RegionNames.CategoryRegion, GetContentDelegate);
        }

        private object GetContentDelegate()
        {
            SyncBaseV view1 = new SyncBaseV();
            view1.DataContext = _containerProvider.Resolve<CategorySyncViewModel>();
            return view1;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ManufacturerService _manufacturerService = new ManufacturerService();
            NopAPIConnect.NopAPIConnect _nopApiConnect = new NopAPIConnect.NopAPIConnect(_configService, _logger);
            containerRegistry.RegisterInstance<IManufacturerService>(_manufacturerService);
            containerRegistry.RegisterInstance<INopAPIConnect>(_nopApiConnect);
        }
    }
}