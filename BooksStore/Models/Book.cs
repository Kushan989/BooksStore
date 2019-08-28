using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BooksStore.Models
{
    [DataContract]
    public class Book
    {

        [DataMember(Name = "Id")]
        public string Id { get; set; }
        [DataMember(Name = "Name")]
        public string BookName { get; set; }
        [DataMember(Name = "Price")]
        public decimal Price { get; set; }
        [DataMember(Name = "Category")]
        public string Category { get; set; }
        [DataMember(Name = "Author")]
        public string Author { get; set; }
    }
}
