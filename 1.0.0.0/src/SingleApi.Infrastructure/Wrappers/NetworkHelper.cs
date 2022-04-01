using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Net.Sockets;

namespace SingleApi.Infrastructure.Wrappers
{
    public class NetworkHelper
    {
        private IHttpContextAccessor httpContextAccessor;
        public NetworkHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        public string GetClientIPAddress()
        {
            string ipAddr = null;
            HttpContext context = httpContextAccessor.HttpContext;

            if (context != null)
            {
                IServerVariablesFeature serverVariables = context.Features.Get<IServerVariablesFeature>();

                if (serverVariables != null)
                {
                    ipAddr = serverVariables["HTTP_X_FORWARDED_FOR"];
                }
                else
                {
                    ipAddr = context.Request.Headers["X-Forwarded-For"];
                }

                if (string.IsNullOrEmpty(ipAddr))
                {
                    if (serverVariables != null)
                    {
                        ipAddr = serverVariables["REMOTE_ADDR"];
                    }
                    else
                    {
                        ipAddr = context.Request.Headers["REMOTE_ADDR"];
                    }

                    ipAddr = ipAddr?.Trim();
                }
                else
                {
                    var addresses = ipAddr.Split(new char[2] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (addresses.Length > 0)
                        ipAddr = addresses[0];
                }
                if (string.IsNullOrEmpty(ipAddr))
                {
                    ipAddr = context.Connection?.RemoteIpAddress?.ToString();
                }
            }

            if (context == null || string.IsNullOrEmpty(ipAddr) || string.Equals(ipAddr, "::1") || string.Equals(ipAddr, "127.0.0.1"))
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                if (host != null && host.AddressList != null)
                {
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddr = ip.ToString();
                            break;
                        }
                    }
                }
            }

            if (ipAddr == null)
                ipAddr = string.Empty;

            return ipAddr;
        }
    }
}
