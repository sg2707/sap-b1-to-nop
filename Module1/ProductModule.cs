using log4net;
using Module1.ViewModels;
using Module1.Views;
using NopAPIConnect;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SAPData;
using SAPData.Services;
using SyncBase.Views;

namespace Module1
{
    public class ProductModule : IModule
    {
        private IContainerProvider _containerProvider;
        private readonly ILog _logger;
        private readonly IConfigSettings _configService;
        public ProductModule(ILog logger, IConfigSettings configService)
        {
            _logger = logger;
            _configService = configService;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(Utilities.RegionNames.ProductRegion, GetContentDelegate);
        }

        private object GetContentDelegate()
        {
            SyncBaseV view1 = new SyncBaseV();
            view1.DataContext = _containerProvider.Resolve<ProductSyncViewModel>();
            return view1;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ProductService _ProductService = new ProductService();
            NopAPIConnect.NopAPIConnect _nopApiConnect = new NopAPIConnect.NopAPIConnect(_configService, _logger);
            containerRegistry.RegisterInstance<IProductService>(_ProductService);
            containerRegistry.RegisterInstance<INopAPIConnect>(_nopApiConnect);
        }
    }
}