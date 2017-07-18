using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Http;
using MyDynamicDns.Filters;
using MyDynamicDns.Services;
using Serilog;

namespace MyDynamicDns.Controllers
{
    [IdentityBasicAuthentication]
    [Authorize]
    public class NicController : ApiController
    {
        [Route("nic/update")]
        [HttpGet]
        public async Task<HttpResponseMessage> Update(string hostname)
        {
            var myip = IpAddressService.GetClientIpAddress();
            return await NicUpdate(hostname, myip);
        }

        [Route("nic/update")]
        [HttpGet]
        public async Task<HttpResponseMessage> Update(string hostname, string myip)
        {
            return await NicUpdate(hostname, myip);
        }

        private static async Task<HttpResponseMessage> NicUpdate(string hostname, string myip)
        {
            try
            {
                var hostAddress = await DnsService.GetHostAddress(hostname);
                var success = hostAddress.Item1;
                var ipAddress = hostAddress.Item2;
                if (!success)
                {
                    Log.Error($"nohost {hostname}");
                    return ReturnStatus("nohost");
                }

                var newIp = IPAddress.Parse(myip);
                if (newIp.AddressFamily != AddressFamily.InterNetwork)
                {
                    Log.Error($"badip {newIp}");
                    return ReturnStatus("badip");
                }

                if (Equals(hostAddress.Item2, newIp))
                {
                    Log.Information($"nochg {hostname} {ipAddress} {newIp}");
                    return ReturnStatus($"nochg {ipAddress}");
                }

                Route53Service.UpdateRoute53Record(hostname, newIp);
                await OpenDnsService.UpdateOpenDnsNetwork(newIp);

                Log.Information($"good {hostname} {ipAddress} {newIp}");
                return ReturnStatus($"good {newIp}");
            }
            catch (Exception ex)
            {
                Log.Fatal($"911 {hostname} {IPAddress.Parse(myip)} {ex.Message} {ex.StackTrace}");
                return ReturnStatus("911");
            }
        }

        private static HttpResponseMessage ReturnStatus(string status)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(status, System.Text.Encoding.UTF8, "text/plain")
            };
        }
    }
}