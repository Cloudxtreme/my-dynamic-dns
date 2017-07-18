using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace MyDynamicDns.Services
{
    public static class OpenDnsService
    {
        public static async Task UpdateOpenDnsNetwork(IPAddress newIp)
        {
            if (CanUpdateOpenDns())
            {
                var httpClient = WebApiApplication.Client;
                var authByteArray = Encoding.ASCII.GetBytes($"{Settings.OpenDnsUserName}:{Settings.OpenDnsPassword}");
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(authByteArray));

                var response =
                    await httpClient.GetAsync($"https://updates.opendns.com/nic/update?hostname=&myip={newIp}");
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    if (content.StartsWith("good"))
                    {
                        Log.Information($"OpenDNS: {content}");
                        return;
                    }

                    Log.Error($"OpenDNS: {content}");
                }

                Log.Error($"OpenDNS: {response.StatusCode} {content}");
            }
        }

        private static bool CanUpdateOpenDns()
        {
            return !string.IsNullOrWhiteSpace(Settings.OpenDnsUserName) &&
                   !string.IsNullOrWhiteSpace(Settings.OpenDnsPassword);
        }
    }
}