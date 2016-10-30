namespace Senparc.Weixin.MP.Sample.CommonService.Data.Models
{
    public class Entity<TEntity> : IEntity<TEntity>
    {
        public Entity() { }

        public Entity(TEntity id)
        {
            this.Id = id;
        }

        public TEntity Id { get; set; }
    }
}
