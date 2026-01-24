using DesafioApi.Data;
using DesafioApi.Entities;
using DesafioApi.Extensions;
using DesafioApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var adminSettings = scope.ServiceProvider.GetRequiredService<IOptions<AdminPadraoSettings>>().Value;

    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario
        {
            NomeUsuario = adminSettings.NomeUsuario,
            Email = adminSettings.Email,
            Senha = adminSettings.Senha
        });
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
