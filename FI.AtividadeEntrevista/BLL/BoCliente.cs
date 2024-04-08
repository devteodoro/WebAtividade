using FI.AtividadeEntrevista.DAL;
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
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public async Task<long> IncluirAsync(Cliente cliente)
        {
            if (!ValidacaoCPF.CPFvalido(cliente.CPF))
                throw new Exception("O CPF do cliente é invalido!");

            if (await VerificarExistenciaAsync(cliente.CPF))
                throw new Exception("Já existe um cliente com o CPF informado cadastrado no sistema!");

            DaoCliente cli = new DaoCliente();
            return await cli.IncluirAsync(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public async Task AlterarAsync(Cliente cliente)
        {
            if (!ValidacaoCPF.CPFvalido(cliente.CPF))
                throw new Exception("O CPF do cliente é invalido!");

            DaoCliente cli = new DaoCliente();
            await cli.AlterarAsync(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public async Task<Cliente> ConsultarAsync(long id)
        {
            DaoCliente cli = new DaoCliente();
            return await cli.ConsultarAsync(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public async Task ExcluirAsync(long id)
        {
            DaoCliente cli = new DaoCliente();
            await cli.ExcluirAsync(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public async Task<List<Cliente>> ListarAsync()
        {
            DaoCliente cli = new DaoCliente();
            return await cli.ListarAsync();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public async Task<listarCliente> PesquisaAsync(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente)
        {
            DaoCliente cli = new DaoCliente();
            return await cli.PesquisaAsync(iniciarEm, quantidade, campoOrdenacao, crescente);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public async Task<bool> VerificarExistenciaAsync(string CPF)
        {
            DaoCliente cli = new DaoCliente();
            return await cli.VerificarExistenciaAsync(CPF);
        }
    }
}
