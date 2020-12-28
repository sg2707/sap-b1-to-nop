using log4net;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SAPData;
using SAPData.Models;
using SAPData.Services;
using SyncBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NopAPIConnect;
using NopAPIConnect.Models;
using AutoMapper;
using Prism.Modularity;
using Prism.Ioc;
using SyncBase.Views;
using VehicleModule.ViewModels;

namespace VehicleModule
{
    public class VehicleModule : IModule
    {
        private IContainerProvider _containerProvider;
    private readonly ILog _logger;
    private readonly IConfigSettings _configService;

    public VehicleModule(ILog logger, IConfigSettings configService)
    {
        _logger = logger;
        _configService = configService;
    }
    public void OnInitialized(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
        var regionManager = containerProvider.Resolve<IRegionManager>();

        regionManager.RegisterViewWithRegion(Utilities.RegionNames.VehicleRegion, GetContentDelegate);
    }

    private object GetContentDelegate()
    {
        SyncBaseV view1 = new SyncBaseV();
        view1.DataContext = _containerProvider.Resolve<VehicleSyncModel>();
        return view1;
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        VehicleService _vehicleService = new VehicleService();
        NopAPIConnect.NopAPIConnect _nopApiConnect = new NopAPIConnect.NopAPIConnect(_configService, _logger);
        containerRegistry.RegisterInstance<IVehicleService>(_vehicleService);
        containerRegistry.RegisterInstance<INopAPIConnect>(_nopApiConnect);
    }
}
}