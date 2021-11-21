using System;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace AtlasIDE
{
    public class Networking
    {
        private static UdpClient udpClient;
        public static void Start()
        {
            udpClient = new UdpClient(1235);
            udpClient.JoinMulticastGroup(IPAddress.Parse("232.1.1.1"), 16);

            var receiveThread = new Thread(Receive);
            receiveThread.Start();
        }

        public static void Receive()
        {
            while (true)
            {
                var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = udpClient.Receive(ref ipEndPoint);

                var Message = Encoding.Default.GetString(data);

                var result = JsonConvert.DeserializeObject<Tweet>(Message);
                switch (result.TweetType) {
                    case "Identity_Thing":
                        result = JsonConvert.DeserializeObject<IdentityThingTweet>(Message);
                        break;
                    case "Identity_Language":
                        result = JsonConvert.DeserializeObject<IdentityLanguageTweet>(Message);
                        break;
                    case "Identity_Entity":
                        result = JsonConvert.DeserializeObject<IdentityEntityTweet>(Message);
                        break;
                    case "Service":
                        result = JsonConvert.DeserializeObject<ServiceTweet>(Message);
                        break;

                    default:
                        break;
                }

                Console.WriteLine(Message);
            }
        }
    }
}