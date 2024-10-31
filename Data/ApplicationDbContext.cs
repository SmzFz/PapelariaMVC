using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PapelariaMVC.Models;

namespace PapelariaMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PapelariaMVC.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<PapelariaMVC.Models.Produto> Produto { get; set; } = default!;
        public DbSet<PapelariaMVC.Models.Fornecedor> Fornecedor { get; set; } = default!;
        public DbSet<PapelariaMVC.Models.Venda> Venda { get; set; } = default!;
    }
}
