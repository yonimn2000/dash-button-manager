using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace YonatanMankovich.DashButtonCore
{
    /// <summary> Provides helper methods to send HTTP requests. </summary>
    public static class WebActionHelpers
    {
        /// <summary> Sends an asynchronous HTTP GET request to the specified URL. </summary>
        public async static Task SendGetRequestAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                await reader.ReadToEndAsync();
        }
    }
}