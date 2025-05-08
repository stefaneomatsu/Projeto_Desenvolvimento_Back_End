-- Tabela Usuarios
INSERT INTO Usuarios(chave, data_criacao, data_alteracao, status, email, nome_usuario, senha_criptografada, perfil_acesso) 
	VALUES (NOVO_GUID_FORMATADO(), CURRENT_TIMESTAMP, NULL, 1, 'ADMIN@TESTE.COM', 'ADMIN', 'xOljPkZ40pRRhH/PWQ7uTg==', 1);

