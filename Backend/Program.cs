using System.Text;
using System.Text.Json.Serialization;
using Backend.Modules;
using Backend.Seed;
using Backend.Services;
using Backend.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
	x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(configurations => 
	{
		configurations.SwaggerDoc("v1", new OpenApiInfo() {Title = "LaboratoriumInformationSystem", Version = "v1"});

		OpenApiSecurityScheme securitySchema = new OpenApiSecurityScheme
		{
			Description =
				"JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.Http,
			Scheme = "bearer",
			Reference = new OpenApiReference
			{
				Type = ReferenceType.SecurityScheme,
				Id = "Bearer"
			}
		};

		configurations.AddSecurityDefinition("Bearer", securitySchema);

		OpenApiSecurityRequirement securityRequirement = new OpenApiSecurityRequirement
		{
			{securitySchema, new[] {"Bearer"}}
		};

		configurations.AddSecurityRequirement(securityRequirement);
	}
);
builder.Services.AddDbContext<LisDbContext>(options =>
	options.UseSqlite(
		builder.Configuration.GetConnectionString("DefaultConnection"),
		optionsBuilder => optionsBuilder.MigrationsAssembly("Database")
	)
);

builder.Services.AddScoped<CheckCategoryModule>();
builder.Services.AddScoped<SampleCategoryModule>();
builder.Services.AddScoped<MedicalToolModule>();
builder.Services.AddScoped<PatientCheckModule>();
builder.Services.AddScoped<PatientSampleModule>();
builder.Services.AddScoped<PatientModule>();
builder.Services.AddScoped(typeof(Module<,>));


builder.Services.AddScoped(typeof(IResponseFactory<>), typeof(ResponseFactory<>));
builder.Services.AddScoped<IRelationCheckerModule, RelationCheckerModule>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Configure Logging
builder.Services.AddLogging(logBuilder =>
	{
		logBuilder.ClearProviders();
		logBuilder.SetMinimumLevel(LogLevel.Information);
		logBuilder.AddNLog("nlog.config");
	});


// Configure Authentication.
builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
.AddJwtBearer(authBuilder =>
	{
		authBuilder.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
			),
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true
		};
	})
.AddCookie("Identity.Application");

builder.Services.AddScoped<TokenService>();

builder.Services.AddIdentityCore<User>(opt => 
{
    opt.Password.RequiredLength = 8;
    opt.SignIn.RequireConfirmedEmail = true;
})
.AddSignInManager()
.AddRoles<IdentityRole>()
.AddTokenProvider<DataProtectorTokenProvider<User>>("Identity")
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<LisDbContext>();

builder.Services.AddAuthorization();

builder.Services.AddSingleton(AutoMapperService.InitializeAutoMapper());

var app = builder.Build();

// Data seeding to create default role and admin user
var services = app.Services.CreateScope().ServiceProvider;
var userManager = services.GetRequiredService<UserManager<User>>();
var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
await DbSeed.Seed(userManager, roleManager);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();