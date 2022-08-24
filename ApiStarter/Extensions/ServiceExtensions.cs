
namespace ApiStarter.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
         services.AddCors(options =>
         {
             options.AddPolicy("CorsPolicy", builder =>
             builder.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
         });


        public static void ConfigureIISIntegration(this IServiceCollection services) =>
         services.Configure<IISOptions>(options =>
         {
         });


        public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddScoped<ILoggerManager, LoggerManager>();


        public static void ConfigureSqlContext(this IServiceCollection services,
                IConfiguration configuration) =>
                 services.AddDbContext<RepositoryContext>(opts =>
                 opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
                b.MigrationsAssembly("ApiStarter")));


        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();



        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
                        builder.AddMvcOptions(config => config.OutputFormatters.Add(new
                        CsvOutputFormatter()));


        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter
                    .SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+json");
                    newtonsoftJsonOutputFormatter
                    .SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+json");
                }
                var xmlOutputFormatter = config.OutputFormatters
               .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter
                    .SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+xml");
                    xmlOutputFormatter
                    .SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+xml");

                }
            });
        }



        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }


        public static void ConfigureResponseCaching(this IServiceCollection services) =>
               services.AddResponseCaching();

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders((expirationOpt) =>
            {
                expirationOpt.MaxAge = 65;
                expirationOpt.CacheLocation = CacheLocation.Private;
            }, (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            });


        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
             {
                 new RateLimitRule
                 {
                     Endpoint = "*",
                     Limit= 30,
                     Period = "5m"
                 }
             };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole),
           builder.Services);
            builder.AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }


        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("secretKey").Value; //Environment.GetEnvironmentVariable("SECRET");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }


        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SomDigital BoilerPlate API",
                    Version = "v1",
                    Description = "BoilerPlate API by Basha SomDigital",
                    TermsOfService = new Uri("https://somdigital.io/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "ARahman Basha",
                        Email = "abdulraxman.basha@gmail.com",
                        Url = new Uri("https://twitter.com/johndoe"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "SomDigital API LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "SomDigital BoilerPlate API",
                    Version = "v2"
                });
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                 {
                     {
                         new OpenApiSecurityScheme
                         {
                            Reference = new OpenApiReference
                         {
                            Type = ReferenceType.SecurityScheme,Id = "Bearer"
                         },
                            Name = "Bearer",
                         },
                         new List<string>()
                         }
                 });
            });

        }
    }
}
