using Newtonsoft.Json;
using System.Collections.Generic;

namespace AtlasIDE
{
    public class App
    {
        public string Name { get; set; }
        public List<object> Commands = new List<object>();
    }
    public class Cond_Eval
    {
        public object IF { get; set; }
        public object THEN { get; set; }
    }

    public class Tweet
    {
        [JsonProperty(PropertyName = "Tweet Type")]
        public string TweetType { get; set; }
        [JsonProperty(PropertyName = "Thing ID")]
        public string ThingID { get; set; }
        [JsonProperty(PropertyName = "Space ID")]
        public string SpaceID { get; set; }
    }


    public class IdentityThingTweet : Tweet
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Model")]
        public string Model { get; set; }
        [JsonProperty(PropertyName = "Vendor")]
        public string Vendor { get; set; }
        [JsonProperty(PropertyName = "Owner")]
        public string Owner { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "OS")]
        public string OS { get; set; }
    }


    public class IdentityLanguageTweet : Tweet
    {
        [JsonProperty(PropertyName = "Network Name")]
        public string NetworkName { get; set; }
        [JsonProperty(PropertyName = "Communication Language")]
        public string CommunicationLanguage { get; set; }
        [JsonProperty(PropertyName = "IP")]
        public string IP { get; set; }
        [JsonProperty(PropertyName = "Port")]
        public string Port { get; set; }
    }

    public class IdentityEntityTweet : Tweet
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "ID")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "Owner")]
        public string Owner { get; set; }
        [JsonProperty(PropertyName = "Vendor")]
        public string Vendor { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
    }


    public class ServiceTweet : Tweet
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Entity ID")]
        public string EntityID { get; set; }
        [JsonProperty(PropertyName = "Vendor")]
        public string Vendor { get; set; }
        [JsonProperty(PropertyName = "API")]
        public string API { get; set; }
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "AppCategory")]
        public string AppCategory { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Keywords")]
        public string Keywords { get; set; }
    }


    public class RelationshipTweet : Tweet
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Owner")]
        public string Owner { get; set; }
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "FS name")]
        public string FSname { get; set; }
        [JsonProperty(PropertyName = "SS name")]
        public string SSname { get; set; }
    }

    public class ServiceCallTweet : Tweet
    {
        [JsonProperty(PropertyName = "Service Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Service Inputs")]
        public string Inputs { get; set; }

    }


    public class ServiceResponseTweet : Tweet
    {
        [JsonProperty(PropertyName = "Service Name")]
        public string ServiceName { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "Status Description")]
        public string StatusDescription { get; set; }
        [JsonProperty(PropertyName = "Service Result")]
        public string ServiceResult { get; set; }
    }

}
