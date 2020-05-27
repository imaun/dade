using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace imun.Dade.Core {
    /// <summary>
    /// Repository for <see cref="T"/> entity.
    /// Provides CRUD and Query methods.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <typeparam name="TKey">Type of entity primary key.</typeparam>
    public interface IDadeSet<T, in TKey> where T : class {

        /// <summary>
        /// Get entity by it's primary key.
        /// </summary>
        /// <param name="id">key</param>
        /// <returns>Entity</returns>
        T Get(TKey id);

        /// <summary>
        /// Get entity by it's primary key asynchronously.
        /// </summary>
        /// <param name="id">key</param>
        /// <returns>Entity</returns>
        Task<T> GetAsync(TKey id);

        /// <summary>
        /// Add entity to DataSet collection.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        long Add(T entity);

        /// <summary>
        /// Add entity to DataSet collection asynchronously.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        Task<int> AddAsync(T entity);

        /// <summary>
        /// Add a list of entities to DataSet collection.
        /// </summary>
        /// <param name="entities">Entities to add.</param>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Add a list of entities to DataSet collection asynchronously.
        /// </summary>
        /// <param name="entities">Entities to add.</param>
        Task AddAsync(IEnumerable<T> entities);

        /// <summary>
        /// Add entity to DataSet for Update.
        /// </summary>
        /// <param name="entity">Entity to Update.</param>
        void Update(T entity);

        /// <summary>
        /// Add entity to DataSet for Update asynchronously.
        /// </summary>
        /// <param name="entity">Entity to Update.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Add a list of entities to DataSet for Update.
        /// </summary>
        /// <param name="entities">Entities to Update.</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Add a list of entities to DataSet for Update asynchronously.
        /// </summary>
        /// <param name="entities">Entities to Update.</param>
        Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Remove entity from DataSet.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        void Delete(T entity);

        /// <summary>
        /// Remove entity from DataSet asynchronously.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Remove a list of entities from DataSet.
        /// </summary>
        /// <param name="entities">Entities for remove.</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Remove a list of entities from DataSet asynchronously.
        /// </summary>
        /// <param name="entities">Entities for remove.</param>
        Task DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Get all the entities from Db.
        /// </summary>
        /// <returns>All the entities from Db.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get all the entities from Db asynchronously.
        /// </summary>
        /// <returns>All the entities from Db.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Query the database.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="param">Sql parameters listed in the query.</param>
        /// <returns></returns>
        IEnumerable<T> Query(string query, object param = null);

        /// <summary>
        /// Query the database asynchronously.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="param">Sql parameters listed in the query.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync(string query, object param = null);

        /// <summary>
        /// Query the database for a single entity based on a condition.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="param">Sql parameters listed in the query.</param>
        /// <returns></returns>
        T Single(string query, object param = null);

        /// <summary>
        /// Query the database for a single entity based on a condition asynchronously.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="param">Sql parameters listed in the query.</param>
        /// <returns></returns>
        Task<T> SingleAsync(string query, object param = null);

        /// <summary>
        /// Determines if a query returns at least one records based on a condition.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="param">Sql parameters listed in the query.</param>
        /// <returns></returns>
        bool Any(string query, object param);

        /// <summary>
        /// Determines if a query returns at least one records based on a condition asynchronously.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="param">Sql parameters listed in the query.</param>
        /// <returns></returns>
        Task<bool> AnyAsync(string query, object param);

        void Execute(string query, object param);

        Task ExecuteAsync(string query, object param);
    }

    public class DadeSet<T, TKey> : IDadeSet<T, TKey> where T : class {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection => Transaction.Connection;

        protected DadeSet(IDbTransaction transaction) {
            Transaction = transaction;
        }

        public virtual T Get(TKey id) {
            return Connection.Get<T>(id);
        }

        public virtual long Add(T entity) {
            return Connection.Insert(entity, Transaction);
        }

        public virtual void Add(IEnumerable<T> entities) {
            Connection.Insert(entities, Transaction);
        }

        public virtual void Update(T entity) {
            Connection.Update(entity, Transaction);
        }

        public virtual void Update(IEnumerable<T> entities) {
            Connection.Update(entities, Transaction);
        }

        public virtual void Delete(T entity) {
            Connection.Delete<T>(entity, Transaction);
        }

        public virtual void Delete(IEnumerable<T> entities) {
            Connection.Delete(entities, Transaction);
        }


        public virtual IEnumerable<T> GetAll() {
            return Connection.GetAll<T>();
        }

        public virtual IEnumerable<T> Query(string query, object param = null) {
            return Connection.Query<T>(query, param, Transaction);
        }

        public virtual T Single(string query, object param = null) {
            return Connection.QuerySingle<T>(query, param);
        }

        public virtual async Task<T> GetAsync(TKey id) {
            return await Connection.GetAsync<T>(id);
        }

        public virtual async Task<int> AddAsync(T entity) {
            return await Connection.InsertAsync(entity, Transaction);
        }

        public virtual async Task AddAsync(IEnumerable<T> entities) {
            await Connection.InsertAsync(entities, Transaction);
        }

        public virtual async Task UpdateAsync(T entity) {
            await Connection.UpdateAsync(entity, Transaction);
        }

        public virtual async Task UpdateAsync(IEnumerable<T> entities) {
            await Connection.UpdateAsync(entities, Transaction);
        }

        public virtual async Task DeleteAsync(T entity) {
            await Connection.DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(IEnumerable<T> entities) {
            await Connection.DeleteAsync(entities, Transaction);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() {
            return await Connection.GetAllAsync<T>();
        }

        public virtual async Task<IEnumerable<T>> QueryAsync(string query, object param = null) {
            return await Connection.QueryAsync<T>(query, param);
        }

        public virtual async Task<T> SingleAsync(string query, object param = null) {
            return await Connection.QuerySingleAsync<T>(query, param);
        }

        public virtual bool Any(string query, object param) {
            return Connection.ExecuteScalar<int>(query) > 0;
        }

        public virtual async Task<bool> AnyAsync(string query, object param) {
            return await Connection.ExecuteScalarAsync<int>(query, param) > 0;
        }

        public void Execute(string query, object param) {
            Connection.Execute(query, param);
        }

        public async Task ExecuteAsync(string query, object param) {
            await Connection.ExecuteAsync(query, param);
        }
    }
}