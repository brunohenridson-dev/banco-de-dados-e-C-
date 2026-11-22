new MySqlConnection(connectionString);

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (IWebHostEnvironment env) =>
{
    var caminhoHtml = Path.Combine(env.ContentRootPath, "bancodedado.html");
    var html = await File.ReadAllTextAsync(caminhoHtml);
    return Results.Content(html, "text/html");
});


app.MapPost("/api/cadastro", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();
    var email =  form["email"].ToString();
    var textinho = form["textinho"].ToString();
    var nome = form["nome"].ToString();
    var waifu =  form["waifu"].ToString();
    var jogo = form["jogo"].ToString();

    string? fotocaminho = null;

   var foto = form.Files.GetFile("foto");
    if(foto is not null && foto.Length > 0)
    {
    var caminho = Path.Combine(app.Environment.ContentRootPath, "guardarfoto");
    Directory.CreateDirectory(caminho);
    var fotoarquivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
    var fusao = Path.Combine(caminho,fotoarquivo);
   

    await using var stream = File.Create(fusao);
    await foto.CopyToAsync(stream);

    fotocaminho = $"guardarfoto/{fotoarquivo}";
    }

    await using var conn = new MySqlConnection(connectionString);
    await conn.OpenAsync();

var flamengo = """
INSERT INTO usuarios (nome, email, waifu, jogo_favorito, descricao, foto_caminho)
VALUES (@nome, @email, @waifu, @jogo_favorito, @descricao, @foto_caminho);
""";

await using var cmd = new MySqlCommand(flamengo, conn);

cmd.Parameters.AddWithValue("@nome", nome);
cmd.Parameters.AddWithValue("@email", email);
cmd.Parameters.AddWithValue("@waifu", waifu);
cmd.Parameters.AddWithValue("@descricao", textinho);
cmd.Parameters.AddWithValue("@foto_caminho", (object?)fotocaminho ?? DBNull.Value);
cmd.Parameters.AddWithValue("@jogo_favorito", jogo);

await cmd.ExecuteNonQueryAsync();

return Results.Ok(new { mensagem = "Cadastro salvo com sucesso!" });

});

app.Run();
