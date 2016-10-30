using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Sample.CommonService.Data.Contracts;
using Senparc.Weixin.MP.Sample.CommonService.Data.Models;
using ServiceStack.OrmLite;

namespace Senparc.Weixin.MP.Sample.CommonService.Data.Repository
{
    public class BaseRepository<T, TKey> : IBaseRepository<T, TKey>
       where T : IEntity<TKey>
    {
        /// <summary>
        /// instance
        /// </summary>
        private static readonly BaseRepository<T, TKey> Instance = new BaseRepository<T, TKey>();

        public static BaseRepository<T, TKey> CreateConnection()
        {
            return Instance;
        }

        /// <summary>
        /// instert entity object to db
        /// </summary>
        /// <param name="item">entity</param>
        /// <returns></returns>
        public virtual long Insert(T item)
        {
            return Utils.ExecuteSqlAction(db => db.Insert(item));
        }

        /// <summary>
        /// Delete entity object to db
        /// </summary>
        /// <param name="id">parmarykey</param>
        /// <returns></returns>
        public virtual int Delete(TKey id)
        {
            return ExecuteSqlAction(db => db.Delete<T>(new { Id = id }));
        }

        /// <summary>
        /// Update entity object to db
        /// </summary>
        /// <param name="item">entity</param>
        /// <returns></returns>
        public virtual int Update(T item)
        {

            return ExecuteSqlAction(db => db.Update(item));
        }

        /// <summary>
        /// query entity from db
        /// </summary>
        /// <param name="id">parmarykey IEntity Id</param>
        /// <returns></returns>
        public virtual T FindById(TKey id)
        {
            return FindList(id).FirstOrDefault();
        }

        /// <summary>
        /// Update entity object from db
        /// </summary>
        /// <param name="id">parmarykey IEntity Id</param>
        /// <returns></returns>
        public virtual List<T> FindList(TKey id)
        {
            return ExecuteSqlAction(db => db.Where<T>(new { Id = id }));
        }

        public static TA ExecuteSqlAction<TA>(Func<IDbConnection, TA> action)
        {
            using (var db = MSSQLProvider.CreateConnection().Open())
            {
                return action(db);
            }
        }
    }
}
