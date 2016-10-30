using System.Collections.Generic;
using Senparc.Weixin.MP.Sample.CommonService.Data.Models;

namespace Senparc.Weixin.MP.Sample.CommonService.Data.Contracts
{
    public interface IBaseRepository<T, TKey> where T : IEntity<TKey>
    {
        /// <summary>
        /// instert entity object to db
        /// </summary>
        /// <param name="item">entity</param>
        /// <returns></returns>
        long Insert(T item);

        /// <summary>
        /// Delete entity object to db
        /// </summary>
        /// <param name="id">parmarykey</param>
        /// <returns></returns>
        int Delete(TKey id);

        /// <summary>
        /// Update entity object to db
        /// </summary>
        /// <param name="item">entity</param>
        /// <returns></returns>
        int Update(T item);

        /// <summary>
        /// query entity from db
        /// </summary>
        /// <param name="id">parmarykey IEntity Id</param>
        /// <returns></returns>
        T FindById(TKey id);

        /// <summary>
        /// Update entity object from db
        /// </summary>
        /// <param name="id">parmarykey IEntity Id</param>
        /// <returns></returns>
        List<T> FindList(TKey id);
    }
}