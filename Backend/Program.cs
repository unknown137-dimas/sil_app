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

// Configure Dependency Injection.
// builder.Services.AddScoped<IRepository<Room>, RoomRepository>();
// builder.Services.AddScoped<IRepository<Reservation>, ReservationRepository>();
// builder.Services.AddScoped<IRepository<Activity>, ActivityRepository>();
 
// builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddLogging(logBuilder =>
	{
		logBuilder.ClearProviders();
		logBuilder.SetMinimumLevel(LogLevel.Information);
		logBuilder.AddNLog("nlog.config");
	});

builder.Services.AddScoped<TokenService>();

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