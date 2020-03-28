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
        void Add(T entity);

        /// <summary>
        /// Add entity to DataSet collection asynchronously.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        Task AddAsync(T entity);

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

    }

    public class DadeSet<T, TKey> : IDadeSet<T, TKey> where T : class {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection => Transaction.Connection;

        protected DadeSet(IDbTransaction transaction) {
            Transaction = transaction;
        }

        public T Get(TKey id) {
            return Connection.Get<T>(id);
        }

        public void Add(T entity) {
            Connection.Insert(entity, Transaction);
        }

        public void Add(IEnumerable<T> entities) {
            Connection.Insert(entities, Transaction);
        }

        public void Update(T entity) {
            Connection.Update(entity, Transaction);
        }

        public void Update(IEnumerable<T> entities) {
            Connection.Update(entities, Transaction);
        }

        public void Delete(T entity) {
            Connection.Delete<T>(entity, Transaction);
        }

        public void Delete(IEnumerable<T> entities) {
            Connection.Delete(entities, Transaction);
        }


        public IEnumerable<T> GetAll() {
            return Connection.GetAll<T>();
        }

        public IEnumerable<T> Query(string query, object param = null) {
            return Connection.Query<T>(query, param, Transaction);
        }

        public T Single(string query, object param = null) {
            return Connection.QuerySingle<T>(query, param);
        }

        public async Task<T> GetAsync(TKey id) {
            return await Connection.GetAsync<T>(id);
        }

        public async Task AddAsync(T entity) {
            await Connection.InsertAsync(entity, Transaction);
        }

        public async Task AddAsync(IEnumerable<T> entities) {
            await Connection.InsertAsync(entities, Transaction);
        }

        public async Task UpdateAsync(T entity) {
            await Connection.UpdateAsync(entity, Transaction);
        }

        public async Task UpdateAsync(IEnumerable<T> entities) {
            await Connection.UpdateAsync(entities, Transaction);
        }

        public async Task DeleteAsync(T entity) {
            await Connection.DeleteAsync(entity);
        }

        public async Task DeleteAsync(IEnumerable<T> entities) {
            await Connection.DeleteAsync(entities, Transaction);
        }

        public async Task<IEnumerable<T>> GetAllAsync() {
            return await Connection.GetAllAsync<T>();
        }

        public async Task<IEnumerable<T>> QueryAsync(string query, object param = null) {
            return await Connection.QueryAsync<T>(query, param);
        }

        public async Task<T> SingleAsync(string query, object param = null) {
            return await Connection.QuerySingleAsync<T>(query, param);
        }

        public bool Any(string query, object param) {
            return Connection.ExecuteScalar<int>(query) > 0;
        }

        public async Task<bool> AnyAsync(string query, object param) {
            return await Connection.ExecuteScalarAsync<int>(query, param) > 0;
        }
    }
}