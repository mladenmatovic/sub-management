namespace SubscriptionManagement.Business.Models
{
    public abstract class Entity
    {
        protected Entity(Guid id) => Id = id;

        public Entity()
        {

        }

        public Guid Id { get; set; }
    }
}
