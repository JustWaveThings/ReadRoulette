using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadRoulette.Domain;
using ReadRoulette.Infra;
using ReadRoulette.Persistence;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>((opts) => opts
            .UseNpgsql(
                builder.Configuration.GetConnectionString("DbConnectionString"),
                b => b.MigrationsAssembly("ReadRoulette")));

        builder.Services.AddTransient<IBookService, BookService>();
        builder.Services.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<AppDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapIdentityApi<IdentityUser>();

        app.Run();
    }
}