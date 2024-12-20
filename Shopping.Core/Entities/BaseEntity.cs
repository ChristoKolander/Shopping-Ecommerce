using System.Text.Json.Serialization;

namespace Shopping.Core.Entities
{
   
    public abstract class BaseEntity<T>
    {
        [JsonPropertyOrder(-3)]
        public virtual T Id { get; set; }
    }
}
