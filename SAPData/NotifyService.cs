using log4net;
using log4net.Appender;
using SAPData.Services;
using SIUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SAPData
{
    public class NotifyService : INotifyService
    {

        class PayLoad
        {
            public string text { get; set; }
        }
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly IConfigSettings _configSettings;
        private readonly ILog _logger;
        public NotifyService(ILog logger, IConfigSettings configSettings)
        {
            _logger = logger;
            _configSettings = configSettings;
        }

        public async Task SendNotification(string subject)
        {
            if (!string.IsNullOrWhiteSpace(_configSettings.EmailNotificationsTo))
            {
                var rootAppender = LogManager.GetRepository()
                                                     .GetAppenders()
                                                     .OfType<FileAppender>()
                                                     .FirstOrDefault(fa => fa.Name == "file");
                string PathToAttach = rootAppender != null ? rootAppender.File : string.Empty;

                EmailManager mailer = new EmailManager();
                if (!String.IsNullOrWhiteSpace(_configSettings.SMTPAddress))
                    mailer = new EmailManager(_configSettings.EmailFrom, _configSettings.EmailPass, _configSettings.SMTPAddress, _configSettings.Port, _configSettings.EnableSSL);
                try
                {
                    mailer.SendEmail(_configSettings.EmailNotificationsTo, subject, "HI,", PathToAttach, _configSettings.EmailSender);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("Error while sending email. Detail: {0}", ex.Message), ex);
                }
            }
            else if (!string.IsNullOrWhiteSpace(_configSettings.SlackWebHookURL))
                await SendSlackNotification(subject);
        }

        private async Task SendSlackNotification(string subject)
        {

            try
            {
                var payload = new PayLoad()
                {
                    text = subject
                };

                var response = await _httpClient.PostAsJsonAsync<PayLoad>(_configSettings.SlackWebHookURL, payload);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Error while sending slack notification. Detail: {0}", ex.Message), ex);

            }
        }
    }
}
