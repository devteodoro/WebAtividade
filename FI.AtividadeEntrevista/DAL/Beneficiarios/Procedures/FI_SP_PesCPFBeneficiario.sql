﻿CREATE PROC FI_SP_PesCPFBeneficiario
    @CPF VARCHAR(14)
AS
BEGIN
	SELECT CPF FROM BENEFICIARIO WHERE CPF = @CPF
END