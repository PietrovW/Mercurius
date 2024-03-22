
## Keycloak


Keycloak configuration: 




```
 services.AddAuthentication(options =>
 {
     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 }).AddJwtBearer(options =>
 {

     options.Authority = "http://localhost:8080/realms/Test2";

     options.MetadataAddress = "http://localhost:8080/realms/Test2/.well-known/openid-configuration";
     options.RequireHttpsMetadata = false;

     options.RequireHttpsMetadata = false; //dev
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateAudience = false,
         ValidateIssuerSigningKey = true,
         ValidateIssuer = true,
         ValidIssuer = "http://localhost:8080/realms/Test2",
         ValidateLifetime = true
     };
     options.Events = new JwtBearerEvents()
     {
         OnAuthenticationFailed = c =>
         {
             c.NoResult();
             c.Response.StatusCode = 401;
             c.Response.ContentType = "text/plain";
             return c.Response.WriteAsync(c.Exception.ToString());
         }

     };
 });
```
