namespace Shopping.Core.Entities
{

    public abstract class BaseEntity<T>
    {
        public virtual T Id { get; set; }
    }
}
