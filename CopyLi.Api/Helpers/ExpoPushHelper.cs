using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CopyLi.Api.Helpers
{
    public static class ExpoPushHelper
    {
        public class NotificationObject
        {
            public string to { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public string sound { get; set; }
            public object data { get; set; }

        }
        public class NotificationData
        {
            public string title { get; set; }
            public string body { get; set; }
            public string id { get; set; }
            public string code { get; set; }
        }
        public static dynamic SendPushNotification(List<NotificationObject> list)
        {
            dynamic body = list.ToArray();
            string response = null;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("accept", "application/json");
                client.Headers.Add("accept-encoding", "gzip, deflate");
                client.Headers.Add("Content-Type", "application/json");
                response = client.UploadString("https://exp.host/--/api/v2/push/send",
                    JsonConvert.SerializeObject(body));
            }
            var json = JsonConvert.DeserializeObject(response);
            return json;
            //return true;
        }
    }
}