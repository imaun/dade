using System.Collections.Generic;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace imun.Dade
{
    /// <summary>
    /// Repository for <see cref="T"/> entity.
    /// Provides CRUD and Query methods.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <typeparam name="TKey">Type of entity primary key.</typeparam>
    public interface IDadeSet<T, in TKey> where T : class
    {
        /// <summary>
        /// Get entity by it's primary key.
        /// </summary>
        /// <param name="id">key</param>
        /// <returns>Entity</returns>
        T Get(TKey id);
        /// <summary>
        /// Add entity to DataSet collection.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        void Add(T entity);
        /// <summary>
        /// Add a list of entities to DataSet collection.
        /// </summary>
        /// <param name="entities">Entities to add.</param>
        void Add(IEnumerable<T> entities);
        /// <summary>
        /// Add entity to DataSet for Update.
        /// </summary>
        /// <param name="entity">Entity to Update.</param>
        void Update(T entity);
        /// <summary>
        /// Add a list of entities to DataSet for Update.
        /// </summary>
        /// <param name="entities">Entities to Update.</param>
        void Update(IEnumerable<T> entities);
        /// <summary>
        /// Remove entity from DataSet.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        void Delete(T entity);
        /// <summary>
        /// Remove a list of entities from DataSet.
        /// </summary>
        /// <param name="entities">Entities for remove.</param>
        void Delete(IEnumerable<T> entities);
        /// <summary>
        /// Get all the entities from Db.
        /// </summary>
        /// <returns>All the entities from Db.</returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Query the database.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="param">Sql parameters listed in the query.</param>
        /// <returns></returns>
        IEnumerable<T> Query(string query, object param = null);
    }

    public class DadeSet<T, TKey> : IDadeSet<T, TKey> where T : class
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection => Transaction.Connection;

        protected DadeSet(IDbTransaction transaction)
        {
            Transaction = transaction;
        }


        public T Get(TKey id)
        {
            return Connection.Get<T>(id);
        }

        public void Add(T entity)
        {
            Connection.Insert(entity, Transaction);
        }

        public void Add(IEnumerable<T> entities)
        {
            Connection.Insert(entities, Transaction);
        }

        public void Update(T entity)
        {
            Connection.Update(entity, Transaction);
        }

        public void Update(IEnumerable<T> entities)
        {
            Connection.Update(entities, Transaction);
        }

        public void Delete(T entity)
        {
            Connection.Delete<T>(entity, Transaction);
        }

        public void Delete(IEnumerable<T> entities)
        {
            Connection.Delete(entities, Transaction);
        }


        public IEnumerable<T> GetAll()
        {
            return Connection.GetAll<T>();
        }

        public IEnumerable<T> Query(string query, object param = null)
        {
            return Connection.Query<T>(query, param, Transaction);
        }
    }
}
