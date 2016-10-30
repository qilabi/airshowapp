namespace Senparc.Weixin.MP.Sample.CommonService.Data.Models
{
    public interface IEntity<TEntity> 
    {
        TEntity Id { get; set; }
    }
}
