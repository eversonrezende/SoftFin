var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Handler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(y => y.FullName);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/v1/requests", (Request request, Handler handler) =>
{
    var response = handler.Handle(request);
    return Results.Created($"/api/v1/requests/{response.Id}", response);
});

app.Run();

public class Request
{
    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int Type { get; set; }

    public decimal Amount { get; set; }

    public long CategoryId { get; set; }

    public string UserId { get; set; } = string.Empty;
}

public class Response
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class Handler
{

    public Response Handle(Request request)
    {
        var response = new Response
        {
            Id = 1,
            Title = request.Title
        };

        return response;
    }
}