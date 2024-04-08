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
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal async Task<long> IncluirAsync(DML.Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("NOME", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("IDCLIENTE", beneficiario.IdCliente),
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_IncluirBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Alterar Beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal async Task AlterarAsync(DML.Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("NOME", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("ID", beneficiario.Id)
            };

            await base.ExecutarAsync("FI_SP_AlterarBeneficiario", parametros);
        }

        /// <summary>
        /// Excluir Beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal async Task ExcluirAsync(long Id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", Id)
            };

            await base.ExecutarAsync("FI_SP_DeletarBeneficiario", parametros);
        }

        /// <summary>
        /// Listar Beneficiarios por cliente
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal async Task<List<Beneficiario>> ListarPorClienteAsync(long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("idCliente", idCliente)
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_ListarBeneficiariosPorCliente", parametros);
            List<DML.Beneficiario> bens = Converter(ds);

            return bens;
        }

        private List<DML.Beneficiario> Converter(DataSet ds)
        {
            List<DML.Beneficiario> lista = new List<DML.Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiario ben = new DML.Beneficiario();
                    ben.Id = row.Field<long>("Id");
                    ben.Nome = row.Field<string>("Nome");
                    ben.CPF = row.Field<string>("CPF");
                    ben.IdCliente = row.Field<long>("IDCLIENTE");
                    lista.Add(ben);
                }
            }
            return lista;
        }

        internal async Task<bool> VerificarExistenciaAsync(string CPF)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", CPF)
            };

            DataSet ds = await base.ConsultarAsync("FI_SP_PesCPFBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }
    }
}
