<h3> 
  Sistema de Gestão Hospitalar e de Serviços de Saúde (SGHSS) - Back-end API RESTful para gestão de pacientes, profissionais, consultas e prontuários em ambientes hospitalares, desenvolvida em .NET com banco de dados Firebird.
</h3>


<h2> 📌 Pré-requisitos </h2>

Antes de executar o projeto, certifique-se de ter instalado:

.NET 6 SDK

Firebird 3.0+ (ou container Docker)

IDE (ex: Visual Studio, VS Code)

<h2>🚀 Como Executar</h2>

<b> 1. Configuração do Banco de Dados </b>

Crie um banco Firebird com o nome SGHSS.fdb (ou ajuste a connection string no arquivo appsettings.json).

Execute os scripts SQL de criação das tabelas (disponíveis em ./scripts/).


<b> 2. Configuração do Projeto </b>
Clone o repositório:

bash
git clone https://github.com/stefaneomatsu/Projeto_Desenvolvimento_Back_End.git
Acesse a pasta do projeto:

bash
cd Projeto_Desenvolvimento_Back_End
Restaure as dependências:

bash
dotnet restore


<b> 3. Rodar a Aplicação </b>
Execute a API:

bash
dotnet run
Acesse a documentação interativa no Swagger:

http://localhost:5000/swagger
