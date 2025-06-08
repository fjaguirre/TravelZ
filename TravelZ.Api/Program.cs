using TravelZ.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelZ.Core.Models;
using TravelZ.Core.Interfaces;
using TravelZ.Core.Services;

void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(opt =>
	{
		opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
		opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			In = ParameterLocation.Header,
			Description = "Please enter token",
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			BearerFormat = "JWT",
			Scheme = "bearer"
		});

		opt.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type=ReferenceType.SecurityScheme,
						Id="Bearer"
					}
				},
				new string[]{}
			}
		});
	});
}

void ConfigureJwt(IServiceCollection services, IConfiguration config)
{
	services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		var jwtKey = config["Jwt:Key"];
		var jwtIssuer = config["Jwt:Issuer"];
		var jwtAudience = config["Jwt:Audience"];

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtIssuer,
			ValidAudience = jwtAudience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
		};
		options.Events = new JwtBearerEvents
		{
			OnChallenge = context =>
			{
				context.HandleResponse();
				context.Response.StatusCode = 401;
				return Task.CompletedTask;
			}
		};
	});
}

async Task StartApp(string[] args)
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Services.AddEndpointsApiExplorer();
	ConfigureSwagger(builder.Services);
	builder.Services.AddDbContext<ApplicationDbContext>();
	builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
	ConfigureJwt(builder.Services, builder.Configuration);
	builder.Services.AddAuthorization();
	builder.Services.AddSingleton<IEmailSender<User>, EmailSender>();
	builder.Services.AddControllers();
	builder.Services.AddAutoMapper(typeof(TravelZ.Core.Mappings.UserProfile).Assembly);
	builder.Services.AddScoped<IUserService, UserService>();

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		await DatabaseSeeder.Seed(app.Services);
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();

	app.Run();
}

await StartApp(args);
