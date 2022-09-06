using Rabobank.TechnicalTest.GCOB.ServiceExtensions;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTechnicalTest();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
	// Set the comments path for the Swagger JSON and UI.
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.DocExpansion(DocExpansion.List);
	options.DefaultModelExpandDepth(5);
	options.DefaultModelRendering(ModelRendering.Model);
	options.EnableFilter();
});

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Run();