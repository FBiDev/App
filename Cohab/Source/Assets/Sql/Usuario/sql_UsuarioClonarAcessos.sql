--
DELETE FROM [DB_COHAB].[dbo].[UsuarioSistema] 
WHERE 1 = 1 
	AND [UsuarioSistema_UsuarioLogin] = @loginDestino;

INSERT INTO [DB_COHAB].[dbo].[UsuarioSistema] 
    ( 
	 [UsuarioSistema_Senha] 
    ,[UsuarioSistema_Ativo] 
    ,[UsuarioSistema_Validade] 
    ,[UsuarioSistema_Free] 
    ,[UsuarioSistema_UsuarioLogin] 
    ,[UsuarioSistema_GrupoId] 
    ,[UsuarioSistema_SistemaSigla] 
    ,[UsuarioSistema_Micro] 
    ,[UsuarioSistema_UsuarioRede] 
    ,[UsuarioSistema_Data] 
    ,[UsuarioSistema_Hora]
	) 
    SELECT 
		@senhaPadrao 
		,1 
		,DATEADD(MONTH, 3, GETDATE()) 
		,5 
		,@loginDestino 
		,[UsuarioSistema_GrupoId] 
		,[UsuarioSistema_SistemaSigla] 
		,[UsuarioSistema_Micro] 
		,[UsuarioSistema_UsuarioRede] 
		,[UsuarioSistema_Data] 
		,[UsuarioSistema_Hora] 
	FROM [DB_COHAB].[dbo].[UsuarioSistema] 
	WHERE 1 = 1 
		AND [UsuarioSistema_UsuarioLogin] = @loginOrigem;
--