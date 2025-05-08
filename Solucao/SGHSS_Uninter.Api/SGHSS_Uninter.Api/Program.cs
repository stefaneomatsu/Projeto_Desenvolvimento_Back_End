using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models.Persistente;
using SGHSS_Uninter.Api.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar DAO
builder.Services.AddScoped<SessaoDAO>();
builder.Services.AddScoped<UsuarioDAO>();
builder.Services.AddScoped<ProfissionalDAO>();
builder.Services.AddScoped<UnidadeDAO>();
builder.Services.AddScoped<PlanoSaudeDAO>();
builder.Services.AddScoped<PacienteDAO>();
builder.Services.AddScoped<ConsultaDAO>();
builder.Services.AddScoped<LaboratorioDAO>();
builder.Services.AddScoped<ExameDAO>();
builder.Services.AddScoped<ProntuarioDAO>();
builder.Services.AddScoped<ProntuarioEntradaDAO>();
builder.Services.AddScoped<PacientesPlanoSaudeDAO>();

// Adicionando os serviços
builder.Services.AddScoped<IUsuarioServico, UsuarioServico>();
builder.Services.AddScoped<IProfissionalServico, ProfissionalServico>();
builder.Services.AddScoped<IUnidadeServico, UnidadeServico>();
builder.Services.AddScoped<IPlanoSaudeServico, PlanoSaudeServico>();
builder.Services.AddScoped<IPacienteServico, PacienteServico>();
builder.Services.AddScoped<IConsultaServico, ConsultaServico>();
builder.Services.AddScoped<IExameServico, ExameServico>();
builder.Services.AddScoped<IProntuarioServico, ProntuarioServico>();
builder.Services.AddScoped<ILaboratorioServico, LaboratorioServico>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
