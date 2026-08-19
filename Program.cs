using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WhatsappWeb.Api.Data;
using WhatsappWeb.Api.Hubs;
using WhatsappWeb.Api.Repositories;
using WhatsappWeb.Api.Services;

Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "1");
Environment.SetEnvironmentVariable("DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE", "false");

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// --- 1. שירותי ליבה (Controllers & Swagger) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. חיבור לבסיס הנתונים (SQLite) ---
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 3. הזרקת תלויות (Dependency Injection) ---
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageService, MessageService>(); 
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileService, FileService>();

// --- 4. הגדרת SignalR ---
builder.Services.AddSignalR();

// --- 5. הגדרת אימות ואבטחה (JWT Authentication) ---
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? builder.Configuration["TokenKey"]
    ?? "super_secret_key_whatsapp_web_clone_123456!"; 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        // טיפול מיוחד ב-SignalR: העברת הטוקן ב-Query String בזמן הידוק חיבור WebSocket
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/chat"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// --- 6. הגדרת CORS עבור צד הלקוח (Angular) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(_ => true) // מאפשר לכל מקור לפנות לשרת
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// --- 7. הגדרת צינור הבקשות (HTTP Request Pipeline / Middleware) ---
app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("CorsPolicy");
app.UseStaticFiles(); // הוספת תמיכה בהגשת קבצים
app.UseAuthentication(); //  מזהה מי המשתמש
app.UseAuthorization();  //  בודק אם יש לו הרשאה

app.MapControllers(); 

// מיפוי ה-SignalR Hub
app.MapHub<ChatHub>("/hubs/chat");

// --- 8. יצירה/עדכון אוטומטי של בסיס הנתונים בהפעלה ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    context.Database.Migrate();
}

app.Run();