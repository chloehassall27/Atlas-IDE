using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace AtlasIDE
{
    class Networking
    {
        private static UdpClient udpClient;
        public static List<Thing> Things { get; } = new List<Thing>();
        public static List<Service> Services { get; } = new List<Service>();
        public static ObservableCollection<Service> ServicesCollection { get; } = new ObservableCollection<Service>();
        public static MainWindow Window;
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

                var tweet = JsonConvert.DeserializeObject<Tweet>(Message);
                switch (tweet.TweetType)
                {
                    case "Identity_Thing":
                        var thingTweet = JsonConvert.DeserializeObject<IdentityThingTweet>(Message);
                        if (Things.Find(x => x.ID == tweet.ThingID) == null)
                        {
                            Console.WriteLine("New thing found!");
                            Things.Add(new Thing(thingTweet));
                            Application.Current.Dispatcher.Invoke(Window.UpdateThings, DispatcherPriority.ContextIdle);
                        }
                        break;
                    case "Identity_Language":
                        var langTweet = JsonConvert.DeserializeObject<IdentityLanguageTweet>(Message);
                        var thing = Things.Find(x => x.ID == langTweet.ThingID);
                        if (thing != null)
                        {
                            thing.AddNetworkInfo(langTweet);
                            Application.Current.Dispatcher.Invoke(Window.UpdateThings, DispatcherPriority.ContextIdle);
                        }
                        break;
                    case "Identity_Entity":
                        var entityTweet = JsonConvert.DeserializeObject<IdentityEntityTweet>(Message);
                        thing = Things.Find(x => x.ID == entityTweet.ThingID);
                        if (thing == null) break;

                        // Seach for entities and add if it doesn't exist
                        if (thing.Entities.Find(x => x.ID == entityTweet.ID) == null)
                        {
                            Console.WriteLine("New entity found!");
                            thing.Entities.Add(new Entity(entityTweet));
                            Application.Current.Dispatcher.Invoke(Window.UpdateThings, DispatcherPriority.ContextIdle);
                        }
                        break;
                    case "Service":
                        var serviceTweet = JsonConvert.DeserializeObject<ServiceTweet>(Message);
                        thing = Things.Find(x => x.ID == serviceTweet.ThingID);
                        if (thing == null) break;

                        var entity = thing.Entities.Find(x => x.ID == serviceTweet.EntityID);
                        if (entity == null) break;

                        // Seach for services and add if it doesn't exist
                        if (entity.Services.Find(x => x.Name == serviceTweet.Name) == null)
                        {
                            Console.WriteLine("New service found!");
                            var service = new Service(serviceTweet);
                            entity.Services.Add(service);
                            Services.Add(service);
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                ServicesCollection.Add(service);
                            });
                            Application.Current.Dispatcher.Invoke(Window.UpdateServices, DispatcherPriority.ContextIdle);
                        }
                        break;

                    case "Relationship":
                        var relationshipTweet = JsonConvert.DeserializeObject<RelationshipTweet>(Message);
                        thing = Things.Find(x => x.ID == relationshipTweet.ThingID);
                        if (thing == null) break;

                        // Seach for relationships and add if it doesn't exist
                        if (thing.Relationships.Find(x => x.Name == relationshipTweet.Name) == null)
                        {
                            Console.WriteLine("New relationship found!");
                            thing.Relationships.Add(new Relationship(relationshipTweet));

                            Application.Current.Dispatcher.Invoke(Window.UpdateRelationship, DispatcherPriority.ContextIdle);
                        }
                        break;

                    // What about unbounded services?

                    default:
                        break;
                }
            }
        }


        private static readonly string HOST = "192.168.0.199";
        private static readonly int PORT = 6668;
        public static ServiceResponseTweet Call(Service service, int? input = null)
        {
            ServiceCallTweet call = new ServiceCallTweet();
            call.TweetType = "Service call";
            call.ThingID = service.ThingID;
            call.SpaceID = service.SpaceID;
            call.Name = service.Name;
            call.Inputs = '(' + input.ToString() + ')';
            Console.WriteLine(JsonConvert.SerializeObject(call, Formatting.Indented));

            Thing thing = Things.Find(x => x.ID == service.ThingID);

            TcpClient client = new TcpClient(HOST, PORT);
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(call, Formatting.Indented));
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);

            data = new Byte[256];
            String responseData = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);
            ServiceResponseTweet response = JsonConvert.DeserializeObject<ServiceResponseTweet>(responseData);
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
            stream.Close();
            client.Close();
            return response;
        }
    }
}