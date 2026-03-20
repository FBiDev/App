--
SELECT 
	  U.Usuario_Matricula 
	, U.Usuario_Nome 
	, U.Usuario_Login 
	, U.Usuario_Email 
	, U.Usuario_Notes 
FROM [vw_lotacaodp] L WITH (nolock) 
	INNER JOIN Usuario U WITH (nolock) ON U.Usuario_Login = L.Usuario_Login 
WHERE 1 = 1 
	AND (COALESCE(@depto, '') = '' OR L.[departamento_sigla] = @depto) 
	AND (COALESCE(@exclusivo, 0) = 0 OR L.[setor_sigla] IS NULL) 
	AND (COALESCE(@ativos, 0) = 0 OR U.usuario_ativo = @ativos)
ORDER BY U.[Usuario_Nome] ASC;
--