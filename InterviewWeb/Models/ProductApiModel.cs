using System;
using System.Runtime.Serialization;

namespace InterviewWeb.Models
{
    // ANSWER:
    [DataContract]
    public class ProductApiModel
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}