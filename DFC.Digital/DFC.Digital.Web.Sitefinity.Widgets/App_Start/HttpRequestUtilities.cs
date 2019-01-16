using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Widgets
{
    public static class HttpRequestUtilities
    {
        public static IPAddress GetIpAddress(this HttpRequestBase request)
        {
            IPAddress address = null;

            if (IPAddress.TryParse(request.Headers["CLIENT-IP"], out address))
            {
                return address;
            }
            else if (request.Headers["X-FORWARDED-FOR"] != null)
            {
                // If the X-FORWARDED-FOR header contains one or more ips - get the first one
                var ips = request.Headers["X-FORWARDED-FOR"].Split(',');
                var ip = ips[0].Trim();
                if (IPAddress.TryParse(ip, out address))
                {
                    return address;
                }
            }

            if (IPAddress.TryParse(request.Headers["X-FORWARDED"], out address))
            {
                return address;
            }
            else if (IPAddress.TryParse(request.Headers["X-CLUSTER-CLIENT-IP"], out address))
            {
                return address;
            }
            else if (IPAddress.TryParse(request.Headers["FORWARDED-FOR"], out address))
            {
                return address;
            }
            else if (IPAddress.TryParse(request.Headers["FORWARDED"], out address))
            {
                return address;
            }
            else if (IPAddress.TryParse(request.UserHostAddress, out address))
            {
                return address;
            }

            return null;
        }
    }
}