using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI Container
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<ToDoListContext>();
    builder.Services.AddScoped<IRepositoryAsync<ToDoItem>, ToDoItemsRepository>();
    builder.Services.AddScoped<IRepositoryAsync<Category>, CategoriesRepository>();
    builder.Services.AddScoped<Seeder>();

    // Lifecycle demo
    builder.Services.AddTransient<IRandomValueServiceTransient, RandomValueServiceTransient>();
    builder.Services.AddScoped<IRandomValueServiceScoped, RandomValueServiceScoped>();
    builder.Services.AddSingleton<IRandomValueServiceSingleton, RandomValueServiceSingleton>();
}

var app = builder.Build();
{
    // Seed data
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
        await seeder.SeedAsync();
    }

    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("v1/swagger.json", "ToDoList API V1");
        config.DocumentTitle = "ToDoList API Documentation";
    });
}

app.Run();
