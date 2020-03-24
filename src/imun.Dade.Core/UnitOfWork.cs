using System;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace imun.Dade.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction Transaction { get; set; }
        void Commit();
        void Rollback();
        void Execute(string sql, object param = null);
        int ExecuteScalar32(string sql, object param = null);
        long ExecuteScalar64(string sql, object param);

    }

    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        public UnitOfWork(IDbConnection connection)
        {
            Transaction = connection.BeginTransaction();
        }


        public void Execute(string sql, object param = null) {
            Transaction.Connection.Execute(sql, param);
        }

        public int ExecuteScalar32(string sql , object param = null) {
            return Transaction.Connection.ExecuteScalar<int>(sql, param);
        }

        public long ExecuteScalar64(string sql, object param) {
            return Transaction.Connection.ExecuteScalar<long>(sql, param);
        }

        public IDbTransaction Transaction { get; set; }
        public void Commit()
        {
            try
            {
                Transaction.Commit();
                Transaction.Connection?.Close();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction?.Dispose();
                Transaction.Connection?.Dispose();
                Transaction = null;
            }
        }

        public void Rollback()
        {
            try
            {
                Transaction.Rollback();
                Transaction.Connection?.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                Transaction?.Dispose();
                Transaction.Connection?.Dispose();
                Transaction = null;
            }
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            dispose(false);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                        Transaction = null;
                    }
                }
                _disposed = true;
            }
        }
    }
}