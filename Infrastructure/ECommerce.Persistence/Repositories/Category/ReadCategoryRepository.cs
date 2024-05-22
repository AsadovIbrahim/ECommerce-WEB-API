using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Persistence.DbContexts;
using ECommerce.Persistence.Repositories.Common;

namespace ECommerce.Persistence.Repositories;

public class ReadCategoryRepository : ReadGenericRepository<Category>, IReadCategoryRepository
{
    public ReadCategoryRepository(ECommerceDbContext context) : base(context)
    {
    }
}
