using MyBud.OrdersApi.HostingExtensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services
    .ConfigureApplicationServices()
    .ConfigureApiVersioningAndExplorer()
    .ConfigureDatabase(config)
    .ConfigureAuth(config)
    .ConfigureCors(config)
    .ConfigureSwagger()
    .ConfigureMvc()
    .AddMvc();

var app = builder.Build();

app.UseConfiguredCors();
app.UseConfiguredContentNegotiation();
//app.UseResponseCompression();
app.UseConfiguredCorrelationContext();
app.UseConfiguredDatabase();
if (app.Environment.IsDevelopment())
    app.UseConfiguredSwagger();
app.UseAuthentication()
   .UseAuthorization();
app.MapControllers();
app.Run();