using Microsoft.EntityFrameworkCore;
using CustomerLeadApi.Data;
using CustomerLeadApi.Services;
using CustomerLeadApi.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// File-based SQLite Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=CustomerLeadDatabase.db");
    options.EnableSensitiveDataLogging();
});

// AutoMapper - Updated configuration
builder.Services.AddAutoMapper(typeof(Program));

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IImageService, ImageService>();

// CORS - More permissive for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Force specific ports
app.Urls.Add("http://localhost:5000");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        bool created = context.Database.EnsureCreated();
        if (created)
        {
            Console.WriteLine("✅ New database created successfully!");
        }
        else
        {
            Console.WriteLine("✅ Database already exists and is ready!");
        }

        SeedDatabase(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error initializing database: {ex.Message}");
        throw;
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Remove HTTPS redirection for now
// app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("🚀 Customer Lead API is running!");
Console.WriteLine("📖 Swagger UI: http://localhost:5000/swagger");
Console.WriteLine("🌐 API Base URL: http://localhost:5000/api");

app.Run();

static void SeedDatabase(ApplicationDbContext context)
{
    try
    {
        if (context.Customers.Any())
        {
            var customerCount = context.Customers.Count();
            Console.WriteLine($"🔄 Database already contains {customerCount} customers. Skipping seed.");
            return;
        }

        var sampleCustomers = new[]
        {
            new CustomerLeadApi.Models.Customer
            {
                Name = "John Doe",
                Email = "john.doe@email.com",
                PhoneNumber = "+1 555-0123",
                Address = "123 Main St, New York, NY 10001",
                ReferralSource = "Google Search",
                Price = 150.00m,
                ContactFrequency = 30,
                StartDate = DateTime.Now.AddDays(7),
                StartTime = new TimeSpan(9, 0, 0),
                EstimatedDuration = 120,
                IsLead = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CustomerLeadApi.Models.Customer
            {
                Name = "Jane Smith",
                Email = "jane.smith@email.com",
                PhoneNumber = "+1 555-0456",
                Address = "456 Oak Ave, Los Angeles, CA 90210",
                ReferralSource = "Friend Referral",
                Price = 200.00m,
                ContactFrequency = 14,
                StartDate = DateTime.Now.AddDays(3),
                StartTime = new TimeSpan(14, 30, 0),
                EstimatedDuration = 90,
                IsLead = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CustomerLeadApi.Models.Customer
            {
                Name = "Mike Johnson",
                Email = "mike.johnson@email.com",
                PhoneNumber = "+1 555-0789",
                Address = "789 Pine St, Chicago, IL 60601",
                ReferralSource = "Facebook Ad",
                Price = 175.00m,
                ContactFrequency = 21,
                IsLead = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Customers.AddRange(sampleCustomers);
        int savedCount = context.SaveChanges();

        Console.WriteLine($"✅ Successfully seeded {savedCount} sample customers!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error seeding database: {ex.Message}");
        Console.WriteLine("⚠️ Application will continue without sample data");
    }
}