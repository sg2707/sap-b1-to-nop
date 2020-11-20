using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Services
{
    public interface IConfigSettings
    {
        string DbServerType { get; set; }
        string LicenseServer { get; set; }
        string SAPUserName { get; set; }
        string SAPPassword { get; set; }

        string EmailNotificationsTo { get; set; }
        string EmailFrom { get; set; }
        string EmailPass { get; set; }
        string SMTPAddress { get; set; }
        int Port { get; set; }
        bool EnableSSL { get; set; }
        string EmailSender { get; }
        string Delimiter { get; }

        string SlackWebHookURL { get; set; }

        string NOP_API_URL { get; set; }
        string NOPUserID { get; set; }
        string NOPPass { get; set; }

        DateTime LastProductSync { get; set; }
        DateTime LastManufactureSync { get; set; }
        DateTime LastSpecSync { get; set; }
        void ReloadSettings();

        void SaveLastProductSync(DateTime lastsynTime);
        void SaveLastManufacturerSync(DateTime value);
        void SaveLastSpecificationSync(DateTime value);
    }
}
