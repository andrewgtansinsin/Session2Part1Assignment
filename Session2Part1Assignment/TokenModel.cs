using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Session2Part1Assignment
{
    public partial class TokenModel
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Token")]
        public Guid Token { get; set; }
    }
}
