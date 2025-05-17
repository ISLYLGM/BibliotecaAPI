using Microsoft.EntityFrameworkCore;
using BibliotecaAPI;

var builder = WebApplication.CreateBuilder(args);

// Configurando o EF Core com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=livros.db"));

var app = builder.Build();

// Método GET - Listar todos os livros
app.MapGet("/livros", (AppDbContext db) =>
{
    var livros = db.TabelaLivros.ToList();
    return Results.Ok(livros);
});

// Método GET por ID - Buscar um livro específico
app.MapGet("/livros/{id}", async (int id, AppDbContext db) =>
{
    var livros = await db.TabelaLivros.FindAsync(id);
    return livros is not null ? Results.Ok(livros) : Results.NotFound("Livro não encontrado, tente novamente!");
});

// Método POST - Criar um novo livro
app.MapPost("/livros", async (Livro livro, AppDbContext db) =>
{
    db.TabelaLivros.Add(livro);
    await db.SaveChangesAsync();
    return Results.Created($"/livros/{livro.Id}", livro);
});

// Método PUT - Atualizar um livro existente
app.MapPut("/livros/{id}", async (int id, Livro input, AppDbContext db) =>
{
    var livro = await db.TabelaLivros.FindAsync(id);
    if (livro is null) return Results.NotFound("Livro não encontrado!");

    livro.Titulo = input.Titulo;
    livro.Autor = input.Autor;
    livro.Ano = input.Ano;

    await db.SaveChangesAsync();
    return Results.Ok(livro);
});

app.Run();
