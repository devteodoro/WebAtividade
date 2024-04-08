using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FI.AtividadeEntrevista.DML;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Cliente
    /// </summary>
    internal class DaoCliente : AcessoDados
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal async Task<long> IncluirAsync(Cliente cliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", cliente.Nome),
                new SqlParameter("Sobrenome", cliente.Sobrenome),
                new SqlParameter("Nacionalidade", cliente.Nacionalidade),
                new SqlParameter("CEP", cliente.CEP),
                new SqlParameter("Estado", cliente.Estado),
                new SqlParameter("Cidade", cliente.Cidade),
                new SqlParameter("Logradouro", cliente.Logradouro),
                new SqlParameter("Email", cliente.Email),
                new SqlParameter("Telefone", cliente.Telefone),
                new SqlParameter("CPF", cliente.CPF)
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_IncClienteV3", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal async Task<Cliente> ConsultarAsync(long Id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", Id)
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_ConsClienteV2", parametros);
            List<Cliente> cli = Converter(ds);

            return cli.FirstOrDefault();
        }

        internal async Task<bool> VerificarExistenciaAsync(string CPF)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", CPF)
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_VerificaCliente", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        internal async Task<listarCliente> PesquisaAsync(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("iniciarEm", iniciarEm),
                new SqlParameter("quantidade", quantidade),
                new SqlParameter("campoOrdenacao", campoOrdenacao),
                new SqlParameter("crescente", crescente)
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_PesqClienteV2", parametros);
            List<Cliente> cli = Converter(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            return new listarCliente { qtd = iQtd, clientes = cli };
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        internal async Task<List<Cliente>> ListarAsync()
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", 0)
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_ConsCliente", parametros);
            List<Cliente> cli = Converter(ds);

            return cli;
        }

        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal async Task AlterarAsync(Cliente cliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", cliente.Nome),
                new SqlParameter("Sobrenome", cliente.Sobrenome),
                new SqlParameter("Nacionalidade", cliente.Nacionalidade),
                new SqlParameter("CEP", cliente.CEP),
                new SqlParameter("Estado", cliente.Estado),
                new SqlParameter("Cidade", cliente.Cidade),
                new SqlParameter("Logradouro", cliente.Logradouro),
                new SqlParameter("Email", cliente.Email),
                new SqlParameter("Telefone", cliente.Telefone),
                new SqlParameter("CPF", cliente.CPF),
                new SqlParameter("ID", cliente.Id)
            };
            await base.ExecutarAsync("FI_SP_AltClienteV2", parametros);
        }

        /// <summary>
        /// Excluir Cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal async Task ExcluirAsync(long Id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", Id)
            };

            await base.ExecutarAsync("FI_SP_DelCliente", parametros);
        }

        private List<DML.Cliente> Converter(DataSet ds)
        {
            List<DML.Cliente> lista = new List<DML.Cliente>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Cliente cli = new DML.Cliente();
                    cli.Id = row.Field<long>("Id");
                    cli.CEP = row.Field<string>("CEP");
                    cli.Cidade = row.Field<string>("Cidade");
                    cli.Email = row.Field<string>("Email");
                    cli.Estado = row.Field<string>("Estado");
                    cli.Logradouro = row.Field<string>("Logradouro");
                    cli.Nacionalidade = row.Field<string>("Nacionalidade");
                    cli.Nome = row.Field<string>("Nome");
                    cli.Sobrenome = row.Field<string>("Sobrenome");
                    cli.Telefone = row.Field<string>("Telefone");
                    cli.CPF = row.Field<string>("CPF");
                    lista.Add(cli);
                }
            }

            return lista;
        }
    }

    public class listarCliente
    {
        public double qtd = 0;
        public List<Cliente> clientes = new List<Cliente>();
    }
}
