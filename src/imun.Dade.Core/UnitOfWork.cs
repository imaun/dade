using System;
using System.Data;

namespace imun.Dade.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction Transaction { get; set; }
        void Commit();
        void Rollback();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        public UnitOfWork(IDbConnection connection)
        {
            Transaction = connection.BeginTransaction();
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