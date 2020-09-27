# Dade
"Dade" is implementation of [UnitOfWork](https://www.martinfowler.com/eaaCatalog/unitOfWork.html) and 
[Repository](https://martinfowler.com/eaaCatalog/repository.html) pattern over [Dapper](https://github.com/StackExchange/Dapper) mini-ORM. 
Dade provides EntityFramework's like way for working with data using Dapper mini-ORM.
You can use this library to achieve UnitOfWork and Repository pattern with Dapper to create your Data-Access layer in some way like EF.

# Sample usage
First thing first, install it via NuGet :
```Install-Package imun.Dade```

 In your data-access layer project, create repository or `DataSet` for each of your entities. Suppose you have two entities `Blog` and `Post`. Add two class named `BlogSet` and `PostSet` which will inherits from `DadeSet` class.
```c#
using imun.Dade;
namespace Blog.Core.Data.Repositories
{
    public class BlogSet: DadeSet<Blog, int>
    {
        protected BlogSet(IDbTransaction transaction) : base(transaction)
        {
        }
    }
}
```
```c#
using imun.Dade;
namespace Blog.Core.Data.Repositories
{
    public class PostSet: DadeSet<Post, int>
    {
        protected PostSet(IDbTransaction transaction) : base(transaction)
        {
        }
    }
}
```
Well, that's it for repositories! Now add another class named `DbContext` or something like that to act as DbContext!

```c#
public class DbContext: DadeContext
{
    public DbContext(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
    {
    }

    public BlogSet Blogs { get; set; }
    public PostSet Posts { get; set; }
}
```

## About
`Dade` means Data in Farsi!
