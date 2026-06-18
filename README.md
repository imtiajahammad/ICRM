## CRM Implementation : The ultimate .net CRM Build from scratch
### A complete, scalable, multi-tenant CRM system
#### What are we building-
- ***Structure***: a large-scale solution
- ***Security***: JWT tokens, auth
- ***Pattern***: Unit of Work, Repository Pattern
- ***Web***: Blazor Web App interacts with API
- ***Tests***: How to write Unit Tests
- ***Scalability***: Microservices Architecture
- ***Mobile App***: iOS and Android using .NET MAUI
- ***Azure***: Deploy to Azure and use GitHub Actions

#### Project Structure:
- ***Blazor Web Project***: The frontend where users interact with the system
- ***API Project***: This handles all incoming requests, Generate and validate token
- ***Service Layer***: This acts as a bridge between the API and the Data Layer
- ***Data Access Library***: This is where our database logic lives
- ***Model Library***: A simple class library that holds all our data models
- ***Utility Library***: A project for various helper functions and tools





#### Step by Step: 
1. Open Terminal and Go to your preferred directory and make a folder for your project solution and open the folder in vscode
    ```
    mkdir ICRM
    cd ICRM
    code .
    ```
2. Create a gitignore file in the solution
    ```
    cd ..
    dotnet new gitignore
    ```
3. Create a readme.md file 
    ```
    code README.md 
    ```
4.  Make a src folder for your project solution
    ```
    mkdir src
    cd src
    ```
5. Create a blank solution with name **ICRM**
    ```
    dotnet new sln -n ICRM
    ```
6. Create a webapi project in the solution
    ```
    dotnet new webapi -n ICRM.APi
    ```
7. Add the project into the solution 
    ```
    dotnet sln add ICRM.Api
    ```
8. Go to ICRM.Api and create a new folder Areas/PublicArea/Controllers
    ```
    cd ICRM.APi
    mkdir Areas
    cd Areas
    mkdir PublicArea
    cd PublicArea
    mkdir Controllers
    cd Controllers
    ```
9. In the Controllers, add a new controller called PublicController
    ```
    code PublicController.cs
    ```
    ```
    using Microsoft.AspNetCore.Mvc;

    namespace ICRM.Api.Areas.PublicArea.Controllers;


    [Area("PublicArea")]
    [DisplayName("Public Controller")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from Public Area");
        }
    }
    ```
10. Add swagger nuget packages into ICRM.Api and adjust swagger in program.cs
    ```
    dotnet add package Swashbuckle.AspNetCore
    ```
    ```
    using Microsoft.OpenApi.Models;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo{Title = "CRM Api", Version = "v1"});
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Api v1"));
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    /*
    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    */
    app.MapControllers();
    app.Run();
    ```
11. Now lets go to src folder and make a Web Blazor app and add it to the solution
    ```
    dotnet new blazor -n ICRM.WebBlazor
    dotnet sln add ICRM.WebBlazor
    ```
12. Go to program.cs file in the Blazor app folder and add the following to connect with the web api
    ```
    var apiBaseAddress = builder.Configuration["ApiBaseAddress"];
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress!)});
    ```
13. Now add ApiBaseAddress into the appSettings.json, take the URL from launchSettings.json from ICRM.Api
    ```
    "ApiBaseAddress" : "http://localhost:5023/api/",
    ```
14. To test the blazor app, lets go to ICRM.WebBlazor/Components/Pages/Home.razor and add the following code-
    ```
    <p>@apiResponse</p>

    @code{
        private string? apiResponse;
        protected override async Task OnInitializedAsync()
        {
            await FetchData();
        }
        private async Task FetchData(){
            try{
                apiResponse = await Http.GetStringAsync("PublicArea/Public");
            }
            catch(Exception ex)
            {
                apiResponse = $"Error: {ex.Message}";
            }
        }
    }
    ```
15. Now api project and web project both to check if blazor get the api response and show it on Home page
16. Go to ICRM.Api project, add required packages and add new classes into Areas/Identity/Data/ApplicationDbContext and Areas/Identity/Data/ApplicationUser
    ```
    dotnet add package Microsoft.AspNetCore.Identity
    dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    ```
    ```
    dotnet new class -n ApplicationDbContext
    dotnet new class -n ApplicationUser
    ```
    ```
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace ICRM.Api;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
    ```
    ```
    using Microsoft.AspNetCore.Identity;
    namespace ICRM.Api;

    public class ApplicationUser : IdentityUser
    {

    }
    ```
17. Added the connectionstring into the ICRM/appsettings.Development.json
    ```
"ConnectionStrings": {
    "ApplicationDbContextConnection": "Server=WIN-ADD-2644-13\\SQLEXPRESS;Database=ICRM-db;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }    ```
18. Add new class library projects-
    ```
    dotnet new classlib -n ICRM.Utility
    dotnet new classlib -n ICRM.Model
    dotnet new classlib -n ICRM.DataAccess
    dotnet new classlib -n ICRM.Service
    ```
    Add the project into the solution 
    ```
    dotnet sln add ICRM.Utility
    dotnet sln add ICRM.Model
    dotnet sln add ICRM.DataAccess
    dotnet sln add ICRM.Service
    ```
19. Add ICRM.Utility reference into Model
    ```
    cd ICRM.Model
    dotnet add reference ../ICRM.Utility
    ```
20. Add ICRM.Model reference into ICRM.DataAccess
    ```
    cd ICRM.DataAccess
    dotnet add reference ../ICRM.Model
    ```
21. Add ICRM.DataAccess reference into ICRM.Service
    ```
    cd ICRM.Service
    dotnet add reference ../ICRM.DataAccess
    ```
22. Add ICRM.Service reference into ICRM.Api
    ```
    cd ICRM.Api
    dotnet add reference ../ICRM.Service
    ```
23. Add nuget package from ICRM.Api to ICRM.Utility
    ```
    dotnet add package System.Text.Json
    ```
24. Add nuget package from ICRM.Api to ICRM.Model
    ```
    dotnet add package Microsoft.AspNetCore.Identity
    dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    ```
25. Add nuget package from ICRM.Api to ICRM.DataAccess
    ```
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    ```
26. Remove the packages from ICRM.Api
    ```
    cd ICRM.Api
    dotnet remove package Microsoft.AspNetCore.Identity
    dotnet remove package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    dotnet remove package Microsoft.EntityFrameworkCore.SqlServer
    dotnet remove package Microsoft.EntityFrameworkCore.Tools
    ```
27. Copy ICRM.API.Areas.Identity.Data.ApplicationDataContext and past it into ICRM.DataAccess. And fix the namespace
28. Create a folder named ***Identity*** in ICRM.Model and move ICRM.Api.Areas.Idenity.Data.ApplicationUser and fix the namespace
    ```
    mkdir Identity
    ```
29. Delete the default created classes(Class1.cs) from ICRM.Model, ICRM.DataAccess, ICRM.Utility, ICRM.Service and delete ***Data*** folder from ICRM.Api
30. Let's add the dbContext into ICRM.Api.Program.cs
    ```
    using ICRM.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;

    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found."); 

    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo{Title = "CRM Api", Version = "v1"});
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Api v1"));
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    /*
    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    */
    app.MapControllers();
    app.Run();
    ```
31. Let's complete the ICRM.Model.ApplicationUser with properties
    ```
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public short? VerificationCode { get; set; }
        public string? ImageName { get; set; }
        public bool? Activity { get; set; }

        [NotMapped]
        public string? FullName => $"{FirstName} {LastName}";
    }
    ```
32. In ICRM.Model, add a new folder ***Enums*** and add new class ***Gender*** 
    ```
    mkdir Enums
    dotnet new class -n "Gender"
    ```
    ```
    public enum Gender
    {
        [EnumMember(Value ="Male")]
        Male,
        [EnumMember(Value ="Female")]    
        Female,
        [EnumMember(Value ="Other")]
        Other
    }
    ```
33. Go to ICRM.Api and add new package 
    ```
    dotnet add package Microsoft.EntityFrameworkCore.Design
    ```
34.  Now select src and run migration command. This will create the migration files
    ```
    dotnet ef migrations add AddIdentity --project ICRM.DataAccess --startup-project ICRM.Api --output-dir Migrations
    ```
35. Run the update-database command and check if db and tables are created or not, specially the migration table with 1 data of migration
    ```
    dotnet ef database update --context ApplicationDbContext --project ICRM.DataAccess --startup-project ICRM.Api
    ```
36. Let's go to ICRM.Api and run the project and check out endpoint with swagger
    ```
    dotnet run
    ```
37. To run the browser itself, let's add the following code in ICRM.Api.program.cs
    ```
        var url = "http://localhost:5023/swagger";
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    ```
#### PHASE: 02






References:
- https://github.com/WeCodersNL/CRM
- https://www.youtube.com/watch?v=rxryM6xtkLA&list=PL77e2l8eKh6kIO0X5fukuVKklseKrf9Yv
    - delete ICRM.API.http file on video 21.11 minutes
- https://www.youtube.com/watch?v=l7taMDegxYw&list=PL77e2l8eKh6kIO0X5fukuVKklseKrf9Yv&index=2
