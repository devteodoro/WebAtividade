using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        ///// <summary>
        ///// Inclui um novo beneficiario
        ///// </summary>
        ///// <param name="cliente">Objeto de cliente</param>
        public async Task<long> IncluirAsync(DML.Beneficiario beneficiario)
        {
            if (!ValidacaoCPF.CPFvalido(beneficiario.CPF))
                throw new Exception($"O CPF do beneficiario {beneficiario.Nome} era invalido");

            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return await ben.IncluirAsync(beneficiario);
        }

        ///// <summary>
        ///// Altera um cliente
        ///// </summary>
        ///// <param name="cliente">Objeto de cliente</param>
        public async Task AlterarAsync(DML.Beneficiario beneficiario)
        {
            if (!ValidacaoCPF.CPFvalido(beneficiario.CPF))
                throw new Exception($"O CPF do beneficiario {beneficiario.Nome} era invalido");

            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            await ben.AlterarAsync(beneficiario);
        }

        ///// <summary>
        ///// Excluir o cliente pelo id
        ///// </summary>
        ///// <param name="id">id do cliente</param>
        ///// <returns></returns>
        public async Task ExcluirAsync(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            await ben.ExcluirAsync(id);
        }

        ///// <summary>
        ///// Lista os clientes
        ///// </summary>
        public async Task<List<DML.Beneficiario>> ListarPorClienteAsync(long idCliente)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return await ben.ListarPorClienteAsync(idCliente);
        }

        ///// <summary>
        ///// VerificaExistencia
        ///// </summary>
        ///// <param name="CPF"></param>
        ///// <returns></returns>
        public async Task<bool> VerificarExistencia(string CPF)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return await ben.VerificarExistenciaAsync(CPF);
        }
    }
}
