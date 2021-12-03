namespace AtlasIDE
{
    class Relationship
    {
        public string ThingID { get; set; }
        public string SpaceID { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        // First Service Name
        public string FSname { get; set; }
        // Second Service Name
        public string SSname { get; set; }

        public string FSargs { get; set; }
        public string SSargs { get; set; }

        public Relationship(RelationshipTweet tweet)
        {
            ThingID = tweet.ThingID;
            SpaceID = tweet.SpaceID;
            Name = tweet.Name;
            Owner = tweet.Owner;
            Category = tweet.Category;
            Type = tweet.Type;
            Description = tweet.Description;
            FSname = tweet.FSname;
            SSname = tweet.SSname;
            FSargs = "";
            SSargs = "";
        }
    }
}
