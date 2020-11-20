
using log4net;
using NopAPIConnect;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SAPData;
using SAPData.Services;
using SpecificationAttributeModule.ViewModels;
using SyncBase.Views;

namespace SpecificationAttributeModule
{
    public class SpecificationAttributeModule : IModule
    {
        private IContainerProvider _containerProvider;
        private readonly ILog _logger;
        private readonly IConfigSettings _configService;

        public SpecificationAttributeModule(ILog logger, IConfigSettings configService)
        {
            _logger = logger;
            _configService = configService;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(Utilities.RegionNames.SpecificationAttributeRegion, GetContentDelegate);
        }

        private object GetContentDelegate()
        {
            SyncBaseV view1 = new SyncBaseV();
            view1.DataContext = _containerProvider.Resolve<SpecificationAttributeSyncViewModel>();
            return view1;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            SpecificationAttributeService _specificationAttributeService = new SpecificationAttributeService();
            NopAPIConnect.NopAPIConnect _nopApiConnect = new NopAPIConnect.NopAPIConnect(_configService, _logger);
            containerRegistry.RegisterInstance<ISpecificationAttributeService>(_specificationAttributeService);
            containerRegistry.RegisterInstance<INopAPIConnect>(_nopApiConnect);
        }
    }
}