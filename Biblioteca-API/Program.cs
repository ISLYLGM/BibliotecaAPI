using Microsoft.EntityFrameworkCore;
using BibliotecaAPI;

var builder = WebApplication.CreateBuilder(args);

// Configurando o EF Core com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=livros.db"));

var app = builder.Build();

// Metódo GET - Listar os livros
app.MapGet("/livros", (AppDbContext db) =>
{
    var livros = db.TabelaLivros.ToList();
    return Results.Ok(livros);
});

// Metódo GET por ID - Listar os livros por ID
app.MapGet("/livros/{id}", async (int id, AppDbContext db) =>
{
    var livros = await db.TabelaLivros.FindAsync(id);
    return livros is not null ? Results.Ok(livros) : Results.NotFound("Livro não encontrado, tente novamente!");
});
app.Run();



