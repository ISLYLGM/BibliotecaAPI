using Microsoft.EntityFrameworkCore;
using BibliotecaAPI;

var builder = WebApplication.CreateBuilder(args);

// Configurando o EF Core com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=livros.db"));

var app = builder.Build();

// Rota de teste
app.MapGet("/livros", () => "Biblioteca funcionando corretamente!");

app.Run();