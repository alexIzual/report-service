using System;
using System.Net.Http;

namespace ReportService.Clients
{
    public class BaseClient : IDisposable
    {
        protected HttpClient HttpClient { get; set; }

        public BaseClient()
        {
            HttpClient = new HttpClient();
        }

        public void Dispose()
        {
            if (HttpClient != null)
                HttpClient.Dispose();
        }
    }
}
