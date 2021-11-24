using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasIDE
{
    class Thing
    {
        public List<Entity> Entities { get; } = new List<Entity>();
        public List<Relationship> Relationships { get; } = new List<Relationship>(); // Technically, relationships are supposed to belong to entities

        public string ID { get; set; }
        public string SpaceID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Vendor { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public string OS { get; set; }
        public string NetworkName { get; set; }
        public string CommunicationLanguage { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }

        public Thing(IdentityThingTweet tweet)
        {
            ID = tweet.ThingID;
            SpaceID = tweet.SpaceID;
            Name = tweet.Name;
            Model = tweet.Model;
            Vendor = tweet.Vendor;
            Vendor = tweet.Vendor;
            Owner = tweet.Owner;
            Description = tweet.Description;
            OS = tweet.OS;
        }

        public void AddNetworkInfo(IdentityLanguageTweet tweet)
        {
            NetworkName = tweet.NetworkName;
            CommunicationLanguage = tweet.CommunicationLanguage;
            IP = tweet.IP;
            Port = tweet.Port;
        }
    }
}
