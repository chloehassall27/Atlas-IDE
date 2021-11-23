using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasIDE
{
    class Service
    {
        public string ThingID { get; set; }
        public string SpaceID { get; set; }
        public string Name { get; set; } // Service Name
        public string EntityID { get; set; }
        public string Vendor { get; set; }
        public string API { get; set; }
        public string Type { get; set; }
        public string AppCategory { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        public Service(ServiceTweet tweet)
        {
            ThingID = tweet.ThingID;
            SpaceID = tweet.SpaceID;
            Name = tweet.Name;
            EntityID = tweet.EntityID;
            Vendor = tweet.Vendor;
            API = tweet.API;
            Type = tweet.Type;
            AppCategory = tweet.AppCategory;
            Description = tweet.Description;
            Keywords = tweet.Keywords;
        }
    }
}
