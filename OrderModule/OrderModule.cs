using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using NopAPIConnect;
using OrderModule.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SAPData;
using SAPData.Services;
using SyncBase.Views;

namespace OrderModule
{
    public class OrderModule : IModule
    {
        private IContainerProvider _containerProvider;
        private readonly ILog _logger;
        private readonly IConfigSettings _configService;
        public OrderModule(ILog logger, IConfigSettings configService)
        {
            _logger = logger;
            _configService = configService;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(Utilities.RegionNames.OrderRegion, GetContentDelegate);
        }

        private object GetContentDelegate()
        {
            SyncBaseV view1 = new SyncBaseV();
            view1.DataContext = _containerProvider.Resolve<OrderSyncViewModel>();
            return view1;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            OrderService _orderService = new OrderService();
            NopAPIConnect.NopAPIConnect _nopApiConnect = new NopAPIConnect.NopAPIConnect(_configService, _logger);
            containerRegistry.RegisterInstance<IOrderService>(_orderService);
            containerRegistry.RegisterInstance<INopAPIConnect>(_nopApiConnect);
        }
    }
}