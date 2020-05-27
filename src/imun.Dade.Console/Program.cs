using imun.Dade.Core;
using System.Data.SQLite;
using System;


namespace imun.Dade.Console {
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("wait...");
            Test();
            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadLine();
        }


        static void Test() {
            var factory = new UnitOfWorkFactory<SQLiteConnection>("Data Source=file.db");

            var db = new Core.DbContext(factory);
            db.CreateDb();
        }
    }
}
