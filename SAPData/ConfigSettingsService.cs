using log4net;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAPData
{
    public class ConfigSettingsService : IConfigSettings
    {
        readonly ILog _logger;
        public ConfigSettingsService(ILog logger)
        {
            _logger = logger;
            ReloadSettings();
        }

        public string DbServerType { get; set; }
        public string LicenseServer { get; set; }
        public string SAPUserName { get; set; }
        public string SAPPassword { get; set; }

        public string EmailNotificationsTo { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPass { get; set; }
        public string SMTPAddress { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string EmailSender { get; set; }
        public string Delimiter { get; set; }

        public string SlackWebHookURL { get; set; }

        public string NOP_API_URL { get; set; }
        public string NOPUserID { get; set; }
        public string NOPPass { get; set; }

        public DateTime LastProductSync { get; set; }
        public DateTime LastManufactureSync { get; set; }
        public DateTime LastSpecSync { get; set; }
        public DateTime LastVehicleSync { get; set; }
        public void ReloadSettings()
        {
            //try
            //{
            var propInfo = typeof(ConfigSettingsService).GetProperties();
            foreach (var property in propInfo)
            {
                object sourceValue = null;
                try
                {
                    sourceValue = ConfigurationManager.AppSettings[property.Name];
                    sourceValue = getConvertedValue(sourceValue, property);
                }
                catch (Exception ex)
                {
                    _logger.Error("ConfigValues: " + property.Name + " unreadable.", ex);
                }

                if (sourceValue != null)
                    property.SetValue(this, sourceValue, null);
            }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex);
            //}         

        }

        private static object getConvertedValue(object sourceValue, PropertyInfo property)
        {
            object setValue = null;

            if (property.PropertyType.IsGenericType && (sourceValue == null || sourceValue.ToString() == ""))
                setValue = null;

            else
            {
                Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                setValue = Convert.ChangeType(sourceValue, t);
            }
            return setValue;
        }

        public void SaveLastProductSync(DateTime value)
        {
            saveConfig(nameof(LastProductSync),value.ToString() );
        }
        public void SaveLastManufacturerSync(DateTime value)
        {
            saveConfig(nameof(LastManufactureSync), value.ToString());
        }
        public void SaveLastSpecificationSync(DateTime value)
        {
            saveConfig(nameof(LastSpecSync), value.ToString());
        }
        public void SaveLastVehicleSync(DateTime value)
        {
            saveConfig(nameof(LastVehicleSync), value.ToString());
        }
        public void saveConfig(string Key, String value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings[Key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");            
            ReloadSettings();
        }
    }
}
