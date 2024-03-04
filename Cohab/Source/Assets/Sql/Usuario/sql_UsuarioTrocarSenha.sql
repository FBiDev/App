--
UPDATE 
	UsuarioSistema 
	SET 
		  UsuarioSistema_Senha = @Senha 
		, UsuarioSistema_Validade = @Validade 
	WHERE 
		UsuarioSistema_UsuarioLogin = @Login 
		AND UsuarioSistema_SistemaSigla = @Sistema;
--