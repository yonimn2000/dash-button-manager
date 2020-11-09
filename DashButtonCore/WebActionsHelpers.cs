using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace YonatanMankovich.DashButtonCore
{
    public static class WebActionsHelpers
    {
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