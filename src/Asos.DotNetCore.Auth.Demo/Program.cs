using Asos.DotNetCore.Auth.Demo.Orders;
using Asos.DotNetCore.Auth.Demo.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IOrderRetriever, OrderRetriever>(client =>
    client.BaseAddress = new Uri("https://orders-api.com/"));

builder.Services.AddMvc(x => x.EnableEndpointRouting = false);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters.NameClaimType = "sub";
        options.MetadataAddress = "https://login.microsoftonline.com/common/.well-known/openid-configuration";
        options.Audience = "https://myapi.audience.com";
        options.MapInboundClaims = false;
    });

builder.Services.AddSingleton<IAuthorizationHandler, SubjectMustMatchRouteParameterHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RouteMustMatchSubject", b => b.Requirements.Add(
            new SubjectMustMatchRouteParameterRequirement("sub", "customerId")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}