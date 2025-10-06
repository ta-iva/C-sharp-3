var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test/{jmeno}", (string jmeno) => $"Hledá se {jmeno} :-)");
app.MapGet("/secti/{a:int}/{b:int}", (int a, int b) => $"Vysledek {a} + {b} = {a + b}");

app.MapGet("/nazdarSvete", () => "Nazdárek!");

app.Run();
