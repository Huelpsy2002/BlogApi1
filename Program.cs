using BlogApi.BussinssLogic;
using BlogApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUsersLogic, UsersLogic>();
builder.Services.AddScoped<IBlogsLogic, BlogsLogic>();
builder.Services.AddScoped<ICommentsLogic,CommentsLogic>();



//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//        {
//            options.RequireHttpsMetadata = false;
//            options.Audience = builder.Configuration["Jwt:Audience"];
//            options.Authority = "https://localhost:7255";
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,

//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                ValidAudience = builder.Configuration["Jwt:Audience"],
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//            };
//            options.Events = new JwtBearerEvents
//            {
//                OnTokenValidated = context =>
//                {
//                    var identity = (ClaimsIdentity)context.Principal.Identity;
//                    var nameIdentifierClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

//                    // Add "sub" claim manually
//                    if (nameIdentifierClaim != null)
//                    {
//                        identity.AddClaim(new Claim("sub", nameIdentifierClaim.Value));
//                    }

//                    return Task.CompletedTask;
//                }
//            };
//        });


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RequireExpirationTime = true,
            ValidateActor = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
