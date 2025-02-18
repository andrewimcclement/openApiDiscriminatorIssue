using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/foo",
           (bool getBar) =>
           {
               Foo foo = getBar ? new Bar(42) : new Baz("Hello, World!");
               return TypedResults.Ok(foo);
           })
   .WithName("GetFoo");

app.Run();

[JsonDerivedType(typeof(Bar), "bar")]
[JsonDerivedType(typeof(Baz), "baz")]
public abstract record Foo;
public record Bar(int Value) : Foo;
public record Baz(string Name) : Foo;