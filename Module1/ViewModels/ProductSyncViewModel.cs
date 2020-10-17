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
using System.Net.Http.Formatting;
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

        public ProductSyncViewModel(ILog logger, IEventAggregator eventAgg, IRegionManager iRegionManager,
                     ISettingService settings, ISAPCompanyService sAPCompanyService, 
                     INotifyService notifyService, INopAPIConnect nopApiConnect, IProductService productService) 
            : base(logger, eventAgg, settings, notifyService)
        {
            _nopApiConnect = nopApiConnect;
            _productService = productService;
            SyncText = "ProdSync";
            Sync = new DelegateCommand(async () => await Task.Run(() => ExecuteSync()));
        }

        public override DelegateCommand Sync { get; set; }
        public override async Task ExecuteSync()
        {
            try
            {
                var sapProducts = _productService.getProductList();

                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<NOPCommerceProduct, NOPCommerceApiProduct>()
                        .ForMember(dest => dest.manufacturer_ids,
                        opt => opt.MapFrom(src => new List<int>() { src.manufacturer_ids } ))
                    );
                //Using automapper
                var mapper = new Mapper(config);
                var apiProducts = mapper.Map<List<NOPCommerceApiProduct>>(sapProducts);
                await _nopApiConnect.SaveProductsAsync(apiProducts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }

}

