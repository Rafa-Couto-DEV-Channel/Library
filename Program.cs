using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*
 Repare que o AppDbContext é registrado no container de dependências via AddDbContext.
 Nesse registro, passamos uma configuração que define qual banco será usado —
 nesse caso, o SQLite, com a connection string vinda do appsettings.json.
 
 Essa configuração é recebida como "options" e repassada para o construtor
 do AppDbContext, que a usa para inicializar o contexto do banco.
*/

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultCOnnection")
));

var app = builder.Build();

app.Run();
