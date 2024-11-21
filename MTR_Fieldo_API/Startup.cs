using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.Service;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Application.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.FileProviders;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace MTR_Fieldo_API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<JwtOptions>(Configuration.GetSection("ApiSettings:JwtOptions"));

        IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
        services.AddSingleton(mapper);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSignalR(options =>
        {
            options.HandshakeTimeout = TimeSpan.FromSeconds(30);
            options.KeepAliveInterval = TimeSpan.FromSeconds(30); // Set to a suitable value
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(60); // Allow more time for client response
            options.EnableDetailedErrors = true; // Useful for debugging
        });
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fieldops-65cc0-firebase-adminsdk-79swg-1ca7e1e46a.json")),
        });
        //services.AddCors(options =>
        //{
        //    options.AddDefaultPolicy(
        //        policy =>
        //        {
        //            policy.AllowAnyOrigin()
        //            .AllowAnyHeader()
        //            .AllowAnyMethod();


        //        });
        //});
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
        // Adding Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        // Adding Jwt Bearer
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = Configuration["ApiSettings:JwtOptions:Audience"],
                ValidIssuer = Configuration["ApiSettings:JwtOptions:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApiSettings:JwtOptions:Secret"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    //var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImJzaEBnbWFpbC5jb20iLCJOYW1lIjoiQmFzaGFyYXQgSHVzc2FpbiIsIkVtYWlsIjoiYnNoQGdtYWlsLmNvbSIsIklkIjoiMSIsIlJvbGUiOiJBZG1pbiIsIm5iZiI6MTcxNTE0NzAxMywiZXhwIjoxNzE1MjMzNDEzLCJpYXQiOjE3MTUxNDcwMTMsImlzcyI6Im10ci1hdXRoLWFwaSIsImF1ZCI6Im10ci1jbGllbnQifQ.IhRxaOjq-c6TwyB26FJeccfIDUHjGR5mMFWvomw9_r4";
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/chatHub")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });



        var serverVersion = new MariaDbServerVersion(new Version(10, 4, 12));
        var connectionString = Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MtrContext>(
            dbContextOption => dbContextOption
                .UseMySql(connectionString, serverVersion));

        //services.AddDbContext<MtrContext>(opt =>
        //{

        //    opt.UseMySQL(connectionString, options => { options.CommandTimeout(180); });


        //});

        services.AddTransient<IAuthenticateService, AuthenticateService>();
        services.AddTransient<ILogService, LogService>();
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITaskService, TaskService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<ICommonService, CommonService>();
        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<IPaymentService, PaymentService>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IWalletService, WalletService>();
        services.AddTransient<ICustomersService, CustomersService>();
        services.AddTransient<IFirebaseNotifications, FirebaseNotifications>();
        services.AddTransient<IAdminFirebaseNotifications, AdminFirebaseNotifications>();
        services.AddTransient<IAdminTaskService, AdminTaskService>();




        UserConnectionManager user = new();
        services.AddSingleton(user);
        services.AddTransient<IMessageService, MessageService>();

        //var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        //services.AddCors(options =>
        //{
        //    options.AddDefaultPolicy(
        //        //name: MyAllowSpecificOrigins,
        //                      policy =>
        //                      {
        //                          //policy.WithOrigins("https://localhost:7038", "https://localhost:44333",
        //                          //                    "http://www.contoso.com")
        //                          policy.AllowAnyOrigin()
        //                          .AllowAnyHeader()
        //                          //.AllowCredentials()
        //                          .AllowAnyMethod(); // add the allowed origins  
        //                      });
        //});
        services.AddTransient<IBankService, BankService>();


        services.AddAuthorization(options =>
        {
            options.AddPolicy("Administrator", policy => policy.RequireClaim("Roles", "Admin"));
        });


        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        //Services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Fieldo API",
                Version = "v1",
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Here Enter JWT Token with bearer format like bearer[space] token"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
        Path.Combine(env.ContentRootPath, "wwwroot")), 
            RequestPath = "", // Empty RequestPath to serve from root
            ServeUnknownFileTypes = true, // Serve files without extensions
            DefaultContentType = "application/json" // Set default content type if needed
        });
        // Enable WebSockets
        app.UseWebSockets(); // Enable WebSockets
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            //c.InjectJavascript("https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js");
            //c.InjectJavascript("https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/5.0.11/signalr.min.js");
            //c.InjectJavascript("/swagger/custom-signalr.js");
           // c.InjectJavascript("/swagger/swagger-extension.js");
        });
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<ChatHub>("/chatHub");
            //steps to connect chathub from postman
            //1.connect through http by changing launchSettings.json config just comment the "sslPort": 44333
            //2.In Postman use websocket call instead of api call
            //3.In Headers pass key "Authorization" and value "Bearer bearer_token_from_signin_api"
            //4.Then click on connect
            GlobalHost.HubPipeline.RequireAuthentication();
        });
    }
}