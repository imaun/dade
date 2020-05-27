using imun.Dade.Core;

namespace imun.Dade.Console.Core {
    public class DbContext: DadeContext {

        public DbContext(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }

        public void CreateDb() {
            string sql = @"CREATE TABLE [Test] ([Id] INTEGER PRIMARY KEY, [Name] NVARCHAR(300) NOT NULL)";
            this.Execute(sql, null);
            this.Commit();
        }
    }
}
