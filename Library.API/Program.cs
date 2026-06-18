using FluentValidation;
using Library.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
const string CorsPolicyName = "LibraryPolicy";

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultCOnnection")
));

builder.Services.AddCors(options =>
        options.AddPolicy(CorsPolicyName, policy =>
        {
            policy.SetIsOriginAllowed(origin =>
            {
                var uri = new Uri(origin);
                return uri.Host == "localhost" || uri.Host == "127.0.0.1";
            });
        }
    )
);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<IClientRepository>(sp => sp.GetRequiredService<ClientRepository>());
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<IBookRepository>(sp => sp.GetRequiredService<BookRepository>());
builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<IReservationRepository>(sp => sp.GetRequiredService<ReservationRepository>());
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Library",
        Version = "v1",
        Description = "Project to P.O.O video class"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

} else
{
    app.UseHsts();
}

app.UseCors(CorsPolicyName);
app.MapControllers();

app.Run();
