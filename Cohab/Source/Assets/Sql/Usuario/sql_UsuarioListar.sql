--
SELECT 
	  Usuario_Login 
	, Usuario_Nome 
	, Usuario_Matricula 
	, Usuario_Ramal 
	, Usuario_Email 
	, Usuario_Chefia 
	, Usuario_Ativo 
	, Usuario_Visivel 
	, Usuario_Notes 
FROM Usuario AS us WITH (NOLOCK) 
WHERE 1 = 1 
	AND (Usuario_Ativo = @Ativo 
		OR (@Ativo = 0 OR @Ativo IS NULL))
	AND (Usuario_Login = @Login 
		OR (@Login = '' OR @Login IS NULL))
	AND (Usuario_Nome LIKE '%'+@Nome+'%' 
		OR (@Nome = '' OR @Nome IS NULL))
ORDER BY 
	Usuario_Nome ASC;
--