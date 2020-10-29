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

namespace CategoryModule.ViewModels
{
    public class CategorySyncViewModel : SyncBaseVM
    {
        public readonly IRegionManager _regionManager;
        private readonly INopAPIConnect _nopApiConnect;
        private readonly ICategoryService _categoryService;
        private readonly IConfigSettings _configSettings;

        public CategorySyncViewModel(ILog logger, IEventAggregator eventAgg, IRegionManager iRegionManager,
                     ISettingService settings,
                     INotifyService notifyService, INopAPIConnect nopApiConnect, ICategoryService categoryService, IConfigSettings configSettings)
            : base(logger, eventAgg, settings, notifyService)
        {
            _nopApiConnect = nopApiConnect;
            _categoryService = categoryService;
            _configSettings = configSettings;
            SyncText = "CategorySync";
            Sync = new DelegateCommand(async () => await Task.Run(() => ExecuteSync()));
        }

        public override DelegateCommand Sync { get; set; }
        public override async Task ExecuteSync()
        {
            try
            { 
                _logger.Info("Starting categories sync");
                var sapCategories = _categoryService.GetCategoryList();
                _logger.Info($"Read ({sapCategories?.Count()}) categories from SAP");
                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<NopCommerceCategory, NopCommerceApiCategory>()
                    );
                //Using automapper
                var mapper = new Mapper(config);
                var apiCategories = mapper.Map<List<NopCommerceApiCategory>>(sapCategories);

                _logger.Info($"Posting ({sapCategories?.Count()}) categories to NOP");

                await _nopApiConnect.SaveCategoriesAsync(apiCategories);
                //save 
                _logger.Info($"Posting ({sapCategories?.Count()}) categories sync completed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }

}


