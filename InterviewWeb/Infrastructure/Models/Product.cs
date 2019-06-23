using System;
using System.ComponentModel.DataAnnotations;

namespace InterviewWeb.Infrastructure.Models
{
    public class Product : IIdentifier
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string InternalCode { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateDiscontinued { get; set; }
    }
}