using System;
using System.Globalization;
using System.Net;

namespace NetHelpers
{
    public static class IPEndpointHelper
    {
        /// <summary>
        /// Create IPEndpoint object from an IPv4 or IPv6 endpoint.
        /// Note: Original code was taken from https://stackoverflow.com/questions/2727609/best-way-to-create-ipendpoint-from-string
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2)
            {
                throw new FormatException("Invalid endpoint format");
            }

            IPAddress ip;
            if (ep.Length > 2)
            {
                // IPv4 address
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                // IPv6 address
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }

            // Port parsing
            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out var port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }
    }
}
