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

namespace ManufacturerModule.ViewModels
{
    public class ManufacturerSyncViewModel : SyncBaseVM
    {
        public readonly IRegionManager _regionManager;
        private readonly INopAPIConnect _nopApiConnect;
        private readonly IManufacturerService _manufacturerService;
        private readonly IConfigSettings _configSettings;

        public ManufacturerSyncViewModel(ILog logger, IEventAggregator eventAgg, IRegionManager iRegionManager,
                     ISettingService settings, ISAPCompanyService sAPCompanyService,
                     INotifyService notifyService, INopAPIConnect nopApiConnect, IManufacturerService manufacturerService, IConfigSettings configSettings)
            : base(logger, eventAgg, settings, notifyService)
        {
            _nopApiConnect = nopApiConnect;
            _manufacturerService = manufacturerService;
            _configSettings = configSettings;
            SyncText = "ManufactureSync";
            Sync = new DelegateCommand(async () => await Task.Run(() => ExecuteSync()));
        }

        public override DelegateCommand Sync { get; set; }
        public override async Task ExecuteSync()
        {
            try
            {
                var StartTime = DateTime.Now;
                _logger.Info("Starting manufacturer sync");
                var sapManufacturer = _manufacturerService.GetManufacturerList(_configSettings.LastManufactureSync);
                _logger.Info($"Read ({sapManufacturer?.Count()}) manufacturers from SAP");
                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<NOPCommerceManufactures, NOPCommerceApiManufactures>()
                    );
                //Using automapper
                var mapper = new Mapper(config);
                var apiManufactures = mapper.Map<List<NOPCommerceApiManufactures>>(sapManufacturer);

                _logger.Info($"Posting ({sapManufacturer?.Count()}) manufacturers to NOP");

                await _nopApiConnect.SaveManufacturesAsync(apiManufactures);
                _configSettings.SaveLastManufacturerSync(StartTime);
                //save 
                _logger.Info($"Posting ({sapManufacturer?.Count()}) manufacturers sync completed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }

}


