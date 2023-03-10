using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Session2Part1Assignment
{
    public class PetModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("photoUrls")]
        public string[] PhotoUrls { get; set; }

        [JsonProperty("tags")]
        public Tags Tags { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Category
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Tags
    {
        [JsonProperty("id")]
        public long[] Id { get; set; }

        [JsonProperty("name")]
        public string[] Name { get; set; }
    }

}
