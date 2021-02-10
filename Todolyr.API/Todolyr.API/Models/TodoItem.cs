using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todolyr.API.Models
{
    public class TodoItem
    {
        [JsonProperty(PropertyName = "id")]
        public string TodoId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }
        [JsonProperty(PropertyName = "owner")]
        public string Owner { get; set; }
    }
}
