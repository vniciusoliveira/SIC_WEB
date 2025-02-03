using API_SIC_WEB_ANGULAR.Controllers;
using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.DAL.PermissaoDeAcessoEloginAntigo;
using API_SIC_WEB_ANGULAR.Interfaces;
using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.Cha

// Service 
builder.Services.AddScoped<PermissaoDeAcessoEloginAntigoService>();
builder.Services.AddScoped<MedidaDisciplinarService>();
builder.Services.AddScoped<GetInfoMaquinaColaborador>();
builder.Services.AddScoped<SenhaService>();
builder.Services.AddScoped<PresencaQueFazADiferencaService>();
builder.Services.AddScoped<PremiacaoIndiqueAmigoService>();
builder.Services.AddScoped<IpInterface, IpService>();
builder.Services.AddScoped<IpService>();
builder.Services.AddScoped<InovAIService>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<LogDal>(); // Registra o LogDal (se ainda não estiver registrado)

// Serviços adicionados para FileManager e FolderManager
builder.Services.AddScoped<FileManagerService>();
builder.Services.AddScoped<FolderManagerService>();

// DAO / DAL
builder.Services.AddScoped<GetInfoColaboradoresDAL>();
builder.Services.AddScoped<MedidaDisciplinarDAL>();
builder.Services.AddScoped<PresencaQueFazADiferencaDAL>();
builder.Services.AddScoped<PremiacaoIndiqueAmigoDAO>();
builder.Services.AddScoped<SenhaDAO>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Obtém o nome do arquivo XML
    string xmlFile = "API_SIC_WEB_ANGULAR.xml";

    // Gera o caminho completo do arquivo XML dentro do diretório base da aplicação
    string filePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // Adiciona o arquivo XML ao Swagger
    options.IncludeXmlComments(filePath);
});


var app = builder.Build();

// Usando CORS
builder.Services.AddCors();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();