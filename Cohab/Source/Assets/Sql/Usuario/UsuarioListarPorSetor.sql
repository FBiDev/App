--
SELECT 
	U.Usuario_Matricula, 
	U.Usuario_Nome, 
	U.Usuario_Login, 
	U.Usuario_Email, 
	U.Usuario_Notes  
FROM [vw_lotacaodp] L WITH (nolock) 
	INNER JOIN Usuario U WITH (nolock) ON U.Usuario_Login = L.Usuario_Login 
WHERE 1 = 1 
	AND (COALESCE(@setor, '') = '' OR l.setor_sigla = @setor OR l.Departamento_Sigla = @setor) 
	AND (U.Usuario_Ativo = @ativos) 
ORDER BY u.Usuario_Nome ASC;
--