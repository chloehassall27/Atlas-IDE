using System.Collections.Generic;

namespace AtlasIDE
{
    class Entity
    {
        public List<Service> Services { get; } = new List<Service>();
        public string ThingID { get; set; }
        public string SpaceID { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }

        public Entity(IdentityEntityTweet tweet)
        {
            ThingID = tweet.ThingID;
            SpaceID = tweet.SpaceID;
            Name = tweet.Name;
            ID = tweet.ID;
            Type = tweet.Type;
            Owner = tweet.Owner;
            Vendor = tweet.Vendor;
            Description = tweet.Description;
        }
    }
}
