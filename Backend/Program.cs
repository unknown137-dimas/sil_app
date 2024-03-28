using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
// builder.Services.AddCors(options =>
// {
// 	options.AddDefaultPolicy(policy =>
// 	{
// 		// TODO change this to the web domain name
// 		policy.WithOrigins("https://localhost:44459");
// 		policy.AllowCredentials();
// 		policy.AllowAnyHeader();
// 		policy.AllowAnyMethod();
// 	});
// });
builder.Services.AddControllers().AddJsonOptions(x =>
	x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ILisDbContext, LisDbContext>(o =>
	o.UseSqlite(
		builder.Configuration.GetConnectionString("DefaultConnection"),
		optionsBuilder => optionsBuilder.MigrationsAssembly("Database")
	)
);

// Configure DB Repositories
builder.Services.AddScoped<IRepository<CheckCategory>, Repository<CheckCategory>>();
builder.Services.AddScoped<IRepository<CheckService>, Repository<CheckService>>();
builder.Services.AddScoped<IRepository<MedicalTool>, Repository<MedicalTool>>();
builder.Services.AddScoped<IRepository<Patient>, Repository<Patient>>();
builder.Services.AddScoped<IRepository<PatientCheck>, Repository<PatientCheck>>();
builder.Services.AddScoped<IRepository<PatientCheckResult>, Repository<PatientCheckResult>>();
builder.Services.AddScoped<IRepository<PatientSample>, Repository<PatientSample>>();
builder.Services.AddScoped<IRepository<PatientSampleResult>, Repository<PatientSampleResult>>();
builder.Services.AddScoped<IRepository<Reagen>, Repository<Reagen>>();
builder.Services.AddScoped<IRepository<SampleCategory>, Repository<SampleCategory>>();
builder.Services.AddScoped<IRepository<SampleService>, Repository<SampleService>>();
 
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

builder.Services.AddIdentityCore<IdentityUser>(opt => 
{
    opt.Password.RequiredLength = 8;
    opt.SignIn.RequireConfirmedEmail = true;
})
.AddSignInManager()
.AddRoles<IdentityRole>()
.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Identity")
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<LisDbContext>();

builder.Services.AddAuthorization();

builder.Services.AddSingleton(AutoMapperService.InitializeAutoMapper());

var app = builder.Build();

// Data seeding to create default role and admin user
var services = app.Services.CreateScope().ServiceProvider;
var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
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