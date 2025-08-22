using ClassLibrary1;
using JasperFx;
using JasperFx.CodeGeneration;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Wolverine;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((hostBuilder, serviceProvider) =>
    {
        // Add services here
    })
    .UseWolverine((opts) =>
    {
        opts.CodeGeneration.ReferenceAssembly(typeof(Command).Assembly);
        opts.Discovery.IncludeAssembly(typeof(Program).Assembly);

        opts.Discovery.IncludeAssembly(typeof(Command).Assembly);
        opts.Discovery.IncludeAssembly(typeof(Program).Assembly);

        opts.Services.CritterStackDefaults(x =>
        {
            x.Production.GeneratedCodeMode = TypeLoadMode.Static;
            x.Production.ResourceAutoCreate = AutoCreate.None;
            x.Production.AssertAllPreGeneratedTypesExist = true;

            x.ApplicationAssembly = typeof(Program).Assembly;

            x.Development.GeneratedCodeMode = TypeLoadMode.Static;
            x.Development.ResourceAutoCreate = AutoCreate.CreateOrUpdate;
            x.Development.AssertAllPreGeneratedTypesExist = true;
        });

        opts.Policies.AutoApplyTransactions();

    });

// This is incredibly hacky, but it allows us to run the code generation in 1 command and in another command run the function host
var runCodeGen = configuration["CodeGen"]?.ToLower() == "true" || args.Contains("codegen");
if (runCodeGen)
{
    host.Build().RunJasperFxCommandsSynchronously(["codegen", "write"]);
    return;
}
else
{
    host.Build().Run();
}


// Alternative Attempt: Using FunctionsApplication.CreateBuilder

//var builder = FunctionsApplication.CreateBuilder(args);

//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .Build();

//// The almost inevitable inclusion of Swashbuckle:)
//builder.Services.AddEndpointsApiExplorer();

//// For now, this is enough to integrate Wolverine into
//// your application, but there'll be *many* more
//// options later of course :-)
//builder.Services.AddWolverine((opts) =>
//{
//    opts.CodeGeneration.ReferenceAssembly(typeof(Command).Assembly);
//    opts.CodeGeneration.ReferenceAssembly(typeof(Program).Assembly);

//    opts.Discovery.IncludeAssembly(typeof(Command).Assembly);
//    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);

//    opts.Services.CritterStackDefaults(x =>
//    {
//        x.Production.GeneratedCodeMode = TypeLoadMode.Static;
//        x.Production.ResourceAutoCreate = AutoCreate.None;
//        x.Production.AssertAllPreGeneratedTypesExist = true;

//        x.ApplicationAssembly = typeof(Program).Assembly;

//        x.Development.GeneratedCodeMode = TypeLoadMode.Static;
//        x.Development.ResourceAutoCreate = AutoCreate.CreateOrUpdate;
//        x.Development.AssertAllPreGeneratedTypesExist = true;
//    });

//    opts.Policies.AutoApplyTransactions();
//});

//var app = builder.Build();
//app.Run();

