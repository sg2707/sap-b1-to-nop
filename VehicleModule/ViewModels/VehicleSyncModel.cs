using AutoMapper;
using log4net;
using NopAPIConnect;
using NopAPIConnect.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SAPData.Models;
using SAPData.Services;
using SyncBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleModule.ViewModels
{
    public class VehicleSyncModel : SyncBaseVM
    {
        public readonly IRegionManager _regionManager;
        private readonly INopAPIConnect _nopApiConnect;
        private readonly IVehicleService _vehicleService;
        private readonly IConfigSettings _configSettings;

        public VehicleSyncModel(ILog logger, IEventAggregator eventAgg, IRegionManager iRegionManager,
                     ISettingService settings, ISAPCompanyService sAPCompanyService,
                     INotifyService notifyService, INopAPIConnect nopApiConnect, IVehicleService vehicleService, IConfigSettings configSettings)
            : base(logger, eventAgg, settings, notifyService)
        {
            _nopApiConnect = nopApiConnect;
            _vehicleService = vehicleService;
            _configSettings = configSettings;
            SyncText = "VehSync";
            Sync = new DelegateCommand(async () => await Task.Run(() => ExecuteSync()));
        }

        public override DelegateCommand Sync { get; set; }
        public override async Task ExecuteSync()
        {
            try
            {
                var StartTime = DateTime.Now;
                _logger.Info("Starting product sync");
                var sapProducts = _vehicleService.GetVehicleList(_configSettings.LastProductSync);
                _logger.Info($"Read ({sapProducts?.Count()}) products from SAP");
                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<NOPCommerceVehicle, NOPCommerceApiVehicle>()
                    //.ForMember(dest => dest.manufacturer_ids,
                    //opt => opt.MapFrom(src => new List<int>() { src.manufacturer_ids } ))
                    // .ForMember(dest => dest.category_ids,
                    //opt => opt.MapFrom(src => new List<int>() { src.category_ids }))
                    );
                //Using automapper
                var mapper = new Mapper(config);
                var apiProducts = mapper.Map<List<NOPCommerceApiVehicle>>(sapProducts);

                //_logger.Info($"Posting ({sapProducts?.Count()}) products to NOP");

                 _nopApiConnect.SaveVehicle(apiProducts);
                //_configSettings.SaveLastProductSync(StartTime);
                ////save 
                //_logger.Info($"Posting ({sapProducts?.Count()}) products sync completed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }

}

