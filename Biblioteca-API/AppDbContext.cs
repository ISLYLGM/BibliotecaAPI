using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI
{
    public class AppDbContext : DbContext
    {
        // Construtor padrão
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Representa a tabela livros no bd
        public DbSet<Livros> TabelaLivros => Set<Livros>();
    }
}