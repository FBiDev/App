--
SELECT 
	Sistema_Nome,
	Sistema_Sigla
	FROM 
		Sistema AS si
	WHERE 
		(Sistema_Proprio = @proprio OR (@proprio = 0 OR @proprio IS NULL))
	ORDER BY 
		Sistema_Sigla ASC;
--