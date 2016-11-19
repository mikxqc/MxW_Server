using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mxw_server
{
    class web
    {
        public static bool CheckWeb(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            if (url.IndexOf(':') < 0)
                url = "http://" + url.TrimStart('/');

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return false;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                        return true;

                    return false;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}
