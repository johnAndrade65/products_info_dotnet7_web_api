using Microsoft.EntityFrameworkCore;

namespace ProductsInfo;

//Conexão entre o banco de dados e a WebAPI
public class ProductsDbContext : DbContext
{
    public DbSet<InfoProducts> InfoProducts { get; set; }
    public DbSet<InfoProductWithFile> InfoProductWithFile { get; set; }

    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : base(options) { }
}
