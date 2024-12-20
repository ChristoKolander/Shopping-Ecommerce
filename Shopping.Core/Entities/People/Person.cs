using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shopping.Core.Entities.People
{
    public abstract class Person : BaseEntity<int>
    {

        [JsonPropertyOrder(-2)]
        public string LastName { get; set; }

        [JsonPropertyOrder(-1)]
        public string FirstName { get; set; }

    }
}
