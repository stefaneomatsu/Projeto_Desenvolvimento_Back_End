/*
 * Script de criação de tabelas
 */

-- Tabela Unidade
CREATE TABLE Unidade (
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	codigo INT NOT NULL,
	nome VARCHAR(255) NOT NULL,
	cnpj VARCHAR(14) NOT NULL,
	endereco VARCHAR(255) NOT NULL
);

CREATE UNIQUE INDEX idx_unidade_codigo ON Unidade (codigo);
CREATE UNIQUE INDEX idx_unidade_cnpj ON Unidade (cnpj);

-- Tabela Usuarios
CREATE TABLE Usuarios (
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	email VARCHAR(255) NOT NULL,
	nome_usuario VARCHAR(100) NOT NULL,
	senha_criptografada VARCHAR(255) NOT NULL,
	perfil_acesso INT NOT NULL
);

CREATE UNIQUE INDEX idx_usuarios_email ON Usuarios (email);
CREATE UNIQUE INDEX idx_usuarios_nome_usuario ON Usuarios (nome_usuario);

-- Tabela Sessao
CREATE TABLE Sessao (
	chave VARCHAR(36) NOT NULL,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	token VARCHAR(255),
	usuario VARCHAR(36) NOT NULL,
	data_saida TIMESTAMP
);

CREATE UNIQUE INDEX idx_sessao_token ON Sessao (token);

-- Tabela Profissionais
CREATE TABLE Profissionais (
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	nome VARCHAR(255) NOT NULL,
	cpf VARCHAR(11) NOT NULL,
	nascimento DATE NOT NULL,
	nr_orgao VARCHAR(255) NOT NULL,
	usuario VARCHAR(36) NOT NULL
);

CREATE UNIQUE INDEX idx_profissionais_cpf ON Profissionais (cpf);
CREATE UNIQUE INDEX idx_profissionais_orgao ON Profissionais (nr_orgao);

-- Tabela PlanoSaude
CREATE TABLE PlanoSaude(
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	nome VARCHAR(255) NOT NULL,
	registro_ans VARCHAR(50)
);

CREATE UNIQUE INDEX idx_planosaude_nome ON PlanoSaude (nome);
CREATE UNIQUE INDEX idx_planosaude_ans ON PlanoSaude (registro_ans);

-- Tabela Pacientes
CREATE TABLE Pacientes(
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	nome VARCHAR(255) NOT NULL,
	cpf VARCHAR(11) NOT NULL,
	nascimento DATE NOT NULL,
	genero INT,
	endereco VARCHAR(255),
	telefone VARCHAR(50),
	usuario VARCHAR(36) NOT NULL
);

CREATE UNIQUE INDEX idx_pacientes_cpf ON Pacientes (cpf);

-- Tabela PacientesPlanoSaude
CREATE TABLE PacientesPlanoSaude(
	chave VARCHAR(36) NOT NULL,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	paciente VARCHAR(36) NOT NULL,
	plano_saude VARCHAR(36) NOT NULL,
	numero_carteira VARCHAR(50) NOT NULL,
	PRIMARY KEY(chave, paciente, plano_saude, numero_carteira)
);

CREATE UNIQUE INDEX idx_pacientes_planosaude ON PacientesPlanoSaude (paciente, plano_saude, numero_carteira);

-- Tabela Consulta
CREATE TABLE Consultas (
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	unidade VARCHAR(36) NOT NULL,
	data_consulta TIMESTAMP NOT NULL,
	tipo INT NOT NULL,
	profissional VARCHAR(36) NOT NULL,
	paciente VARCHAR(36) NOT NULL,
	link_consulta VARCHAR(255)
);

CREATE UNIQUE INDEX idx_consultas ON Consultas (unidade, data_consulta, profissional);

-- Tabela Laboratorio
CREATE TABLE Laboratorio(
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	nome VARCHAR(255) NOT NULL,
	endereco VARCHAR(255) NOT NULL,
	cnpj VARCHAR(14) NOT NULL
);

CREATE UNIQUE INDEX idx_laboratorio_cnpj ON Laboratorio (cnpj);

-- Tabela Exames
CREATE TABLE Exames(
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	paciente VARCHAR(36) NOT NULL,
	profissional VARCHAR(36) NOT NULL,
	tipo INT NOT NULL,
	descricao VARCHAR(255),
	data_solicitacao TIMESTAMP NOT NULL,
	data_realizacao TIMESTAMP,
	laboratorio VARCHAR(36) NOT NULL,
	anexo BLOB
);

CREATE UNIQUE INDEX idx_exames ON Exames (paciente, profissional, tipo, data_solicitacao);

-- Tabela Prontuarios
CREATE TABLE Prontuarios(
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	paciente VARCHAR(36) NOT NULL,
	historico_medico VARCHAR(255),
	alergias VARCHAR(255),
	medicamentos VARCHAR(255),
	cirurgias VARCHAR(255)
);

CREATE UNIQUE INDEX idx_prontuarios_paciente ON Prontuarios (paciente);

-- Tabela ProntuariosEntradas
CREATE TABLE ProntuariosEntradas(
	chave VARCHAR(36) NOT NULL PRIMARY KEY,
	data_criacao TIMESTAMP NOT NULL,
	data_alteracao TIMESTAMP,
	status SMALLINT NOT NULL,
	prontuario VARCHAR(36) NOT NULL,
	profissional VARCHAR(36) NOT NULL,
	tipo INT NOT NULL,
	chave_vinculo VARCHAR(36),
	data_entrada TIMESTAMP NOT NULL,
	titulo VARCHAR(100) NOT NULL,
	descricao VARCHAR(255) NOT NULL,
	anexo BLOB
);

CREATE UNIQUE INDEX idx_prontuarios_entradas ON ProntuariosEntradas (prontuario, profissional, data_entrada, tipo);
