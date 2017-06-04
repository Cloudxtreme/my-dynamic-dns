using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyDynamicDns.Services
{
    public static class DnsService
    {
        public static async Task<Tuple<bool, IPAddress>> GetHostAddress(string hostname)
        {
            try
            {
                var hostAddresses = await Dns.GetHostAddressesAsync(hostname);
                return new Tuple<bool, IPAddress>(true, hostAddresses[0]);
            }
            catch (SocketException)
            {
                return new Tuple<bool, IPAddress>(false, null);
            }
        }
    }
}