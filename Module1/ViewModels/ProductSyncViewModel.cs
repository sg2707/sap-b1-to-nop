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
using Newtonsoft.Json;
using NopAPIConnect;
using NopAPIConnect.Models;
using AutoMapper;

namespace Module1.ViewModels
{
    public class ProductSyncViewModel : SyncBaseVM
    {
        public readonly IRegionManager _regionManager;
        private readonly INopAPIConnect _nopApiConnect;
        private readonly IProductService _productService;
        private readonly IConfigSettings _configSettings;

        public ProductSyncViewModel(ILog logger, IEventAggregator eventAgg, IRegionManager iRegionManager,
                     ISettingService settings, ISAPCompanyService sAPCompanyService, 
                     INotifyService notifyService, INopAPIConnect nopApiConnect, IProductService productService, IConfigSettings configSettings) 
            : base(logger, eventAgg, settings, notifyService)
        {
            _nopApiConnect = nopApiConnect;
            _productService = productService;
            _configSettings = configSettings;
            SyncText = "ProdSync";
            Sync = new DelegateCommand(async () => await Task.Run(() => ExecuteSync()));
        }

        public override DelegateCommand Sync { get; set; }
        public override async Task ExecuteSync()
        {
            try
            {
                var StartTime = DateTime.Now;
                _logger.Info("Starting product sync");
                var sapProducts = _productService.GetProductList(_configSettings.LastProductSync);
                _logger.Info($"Read ({sapProducts?.Count()}) products from SAP");
                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<NOPCommerceProduct, NOPCommerceApiProduct>()
                        //.ForMember(dest => dest.manufacturer_ids,
                        //opt => opt.MapFrom(src => new List<int>() { src.manufacturer_ids } ))
                        // .ForMember(dest => dest.category_ids,
                        //opt => opt.MapFrom(src => new List<int>() { src.category_ids }))
                    );
                //Using automapper
                var mapper = new Mapper(config);
                var apiProducts = mapper.Map<List<NOPCommerceApiProduct>>(sapProducts);

                _logger.Info($"Posting ({sapProducts?.Count()}) products to NOP");

                await _nopApiConnect.SaveProductsAsync(apiProducts);
                _configSettings.SaveLastProductSync(StartTime);
                //save 
                _logger.Info($"Posting ({sapProducts?.Count()}) products sync completed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }

}

