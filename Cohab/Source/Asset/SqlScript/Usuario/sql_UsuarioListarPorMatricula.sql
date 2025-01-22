--
SELECT 
	  U.Usuario_Login 
	, U.Usuario_Nome 
	, U.Usuario_Matricula 
	, U.Usuario_Ramal 
	, U.Usuario_Email 
	, U.Usuario_Chefia 
	, U.Usuario_Ativo 
	, U.Usuario_Visivel 
	, U.Usuario_Notes 
FROM [vw_lotacaodp] L WITH (nolock) 
	INNER JOIN Usuario U WITH (nolock) ON U.Usuario_Login = L.Usuario_Login 
WHERE 1 = 1 
	AND (COALESCE(@matricula, '') = '' OR L.[matricula] = @matricula) 
	AND (COALESCE(@login, '') = '' OR U.[usuario_login] = @login) 
ORDER BY U.[Usuario_Nome] ASC;
--