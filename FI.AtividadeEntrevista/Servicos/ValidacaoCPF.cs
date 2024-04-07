using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Servicos
{
    public static class ValidacaoCPF
    {
        public static bool CPFvalido(string cpf)
        {
            if(string.IsNullOrEmpty(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || !long.TryParse(cpf, out long result))
                return false;

            string cpfSemDigitos = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpfSemDigitos[i].ToString()) * (10 - i);
            }

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            }

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digitoVerificador2;
        }
    }
}
