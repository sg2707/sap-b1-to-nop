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

namespace SpecificationAttributeModule.ViewModels
{
    public class SpecificationAttributeSyncViewModel : SyncBaseVM
    {
        public readonly IRegionManager _regionManager;
        private readonly INopAPIConnect _nopApiConnect;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IConfigSettings _configSettings;

        public SpecificationAttributeSyncViewModel(ILog logger, IEventAggregator eventAgg, IRegionManager iRegionManager,
                     ISettingService settings, ISAPCompanyService sAPCompanyService,
                     INotifyService notifyService, INopAPIConnect nopApiConnect, ISpecificationAttributeService specificationAttributeService, IConfigSettings configSettings)
            : base(logger, eventAgg, settings, notifyService)
        {
            _nopApiConnect = nopApiConnect;
            _specificationAttributeService = specificationAttributeService;
            _configSettings = configSettings;
            SyncText = "SpecAttributeSync";
            Sync = new DelegateCommand(async () => await Task.Run(() => ExecuteSync()));
        }

        public override DelegateCommand Sync { get; set; }
        public override async Task ExecuteSync()
        {
            try
            {
                var StartTime = DateTime.Now;
                _logger.Info("Starting specification attribute sync"); 
                var sapSpecAttribute = _specificationAttributeService.GetSpecificationAttributeList(_configSettings.LastSpecSync);
                _logger.Info($"Read ({sapSpecAttribute?.Count()}) specification attribute from SAP");
                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<NOPCommerceSpecificationAttribute, NOPCommerceApiSpecificationAttribute>()
                    );
                //Using automapper
                var mapper = new Mapper(config);
                var apiSpecAttribute = mapper.Map<List<NOPCommerceApiSpecificationAttribute>>(sapSpecAttribute);

                _logger.Info($"Posting ({sapSpecAttribute?.Count()}) specification attribute to NOP");

                await _nopApiConnect.SaveSpecificationAttributeAsync(apiSpecAttribute);
                _configSettings.SaveLastSpecificationSync(StartTime);
                //save 
                _logger.Info($"Posting ({sapSpecAttribute?.Count()}) specification attribute sync completed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }

}



