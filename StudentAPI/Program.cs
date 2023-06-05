using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentAPI.DAL;
using StudentAPI.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Configure Identity options if needed
})
    .AddEntityFrameworkStores<StudentDBContext>();


builder.Services.AddScoped<Student_DAL>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "Standard Authorization header using Bearer scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name="Authorization",
        Type= SecuritySchemeType.ApiKey 
        
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

//Dependency Injection of DbContext Class
builder.Services.AddDbContext<StudentDBContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection")));


// Configure authentication and JWT bearer options
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value);   
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,        
        ValidateIssuerSigningKey = true,
                
    };
});

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("https://localhost:44302").AllowAnyMethod().AllowAnyHeader();

    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NgOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
