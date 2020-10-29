
using log4net;
using ManufacturerModule.ViewModels;
using NopAPIConnect;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SAPData;
using SAPData.Services;
using SyncBase.Views;


namespace ManufacturerModule
{
    public class ManufacturerModule : IModule
    {
        private IContainerProvider _containerProvider;
        private readonly ILog _logger;
        private readonly IConfigSettings _configService;
        public ManufacturerModule(ILog logger, IConfigSettings configService)
        {
            _logger = logger;
            _configService = configService;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(Utilities.RegionNames.ManufacturerRegion, GetContentDelegate);
        }

        private object GetContentDelegate()
        {
            SyncBaseV view1 = new SyncBaseV();
            view1.DataContext = _containerProvider.Resolve<ManufacturerSyncViewModel>();
            return view1;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            CategoryService _categoryService = new CategoryService();
            NopAPIConnect.NopAPIConnect _nopApiConnect = new NopAPIConnect.NopAPIConnect(_configService, _logger);
            containerRegistry.RegisterInstance<ICategoryService>(_categoryService);
            containerRegistry.RegisterInstance<INopAPIConnect>(_nopApiConnect);
        }
    }
}