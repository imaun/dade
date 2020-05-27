using System;
using System.Data;

namespace imun.Dade.Core {
    public interface IUnitOfWorkFactory {
        IUnitOfWork Create();
    }

    public class UnitOfWorkFactory<TConnection> : 
        IUnitOfWorkFactory where TConnection : IDbConnection, new() {
        
        private string connectionString;

        public UnitOfWorkFactory(string connectionString) {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString cannot be null");
            
            this.connectionString = connectionString;
        }

        public IUnitOfWork Create() {
            return new UnitOfWork(CreateConnection());
        }

        private IDbConnection CreateConnection() {
            var connection = new TConnection { ConnectionString = connectionString };
            return connection;
        }
    }
}