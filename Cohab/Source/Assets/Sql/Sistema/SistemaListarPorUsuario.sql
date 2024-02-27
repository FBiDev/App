﻿--
SELECT 
	Sistema_Nome,
	Sistema_Sigla
	FROM 
		Sistema AS si
		LEFT JOIN UsuarioSistema AS us 
			ON si.Sistema_Sigla = us.UsuarioSistema_SistemaSigla
	WHERE 
		us.UsuarioSistema_UsuarioLogin = @Login
	ORDER BY 
		Sistema_Sigla ASC;
--