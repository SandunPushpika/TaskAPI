using TaskAPI.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Authentication
builder.Services.AddJwtAuthConfiguration(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], builder.Configuration["Jwt:Key"]);

// Configure Services
builder.Services.AddAllServices();

// CORS Configuration
builder.Services.AddCors(cors => {
    cors.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
