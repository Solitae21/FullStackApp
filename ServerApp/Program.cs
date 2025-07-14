using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services for performance optimization
// Copilot helped identify these services for better performance and caching
builder.Services.AddCors();
builder.Services.AddMemoryCache(); // Added caching for performance
builder.Services.AddOutputCache(); // Added output caching for API responses

var app = builder.Build();

// Configure CORS with more specific settings for security
// Copilot suggestion: Use specific origins instead of AllowAnyOrigin for production
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:5273", "https://localhost:7299") // Specific origins for security
          .AllowAnyMethod()
          .AllowAnyHeader());

// Add output caching middleware for performance
app.UseOutputCache();

// Optimized API endpoint with caching and structured response
// Copilot assisted in creating this cached endpoint for better performance
app.MapGet("/api/productlist", () =>
{
    // Simulated data - in real application, this would come from a database
    // Copilot helped structure this data model for optimal JSON serialization
    var products = new[]
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200.50,
            Stock = 25,
            Category = new Category { Id = 101, Name = "Electronics" },
            Description = "High-performance laptop for professionals",
            ImageUrl = "/images/laptop.jpg"
        },
        new Product
        {
            Id = 2,
            Name = "Headphones",
            Price = 50.00,
            Stock = 100,
            Category = new Category { Id = 102, Name = "Accessories" },
            Description = "Premium wireless headphones",
            ImageUrl = "/images/headphones.jpg"
        },
        new Product
        {
            Id = 3,
            Name = "Smartphone",
            Price = 899.99,
            Stock = 50,
            Category = new Category { Id = 101, Name = "Electronics" },
            Description = "Latest smartphone with advanced features",
            ImageUrl = "/images/smartphone.jpg"
        }
    };

    return Results.Ok(new { 
        Products = products, 
        TotalCount = products.Length,
        Timestamp = DateTime.UtcNow
    });
})
.CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(5))) // Cache for 5 minutes to reduce server load
.WithName("GetProducts")
.WithOpenApi(); // Added OpenAPI documentation

// Health check endpoint for monitoring
// Copilot suggested adding this for better observability
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

app.Run();

// Data models for better type safety and IntelliSense
// Copilot helped create these models with proper JSON serialization attributes
public record Product
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public double Price { get; init; }
    public int Stock { get; init; }
    public Category Category { get; init; } = new();
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}

public record Category
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}