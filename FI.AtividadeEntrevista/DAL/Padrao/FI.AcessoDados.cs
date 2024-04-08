using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DAL
{
    internal class AcessoDados
    {
        private string stringDeConexao
        {
            get
            {
                ConnectionStringSettings conn = System.Configuration.ConfigurationManager.ConnectionStrings["BancoDeDados"];
                if (conn != null)
                    return conn.ConnectionString;
                else
                    return string.Empty;
            }
        }

        internal async Task ExecutarAsync(string nomeProcedure, List<SqlParameter> parametros)
        {
            using (SqlConnection conexao = new SqlConnection(stringDeConexao))
            {
                using (SqlCommand comando = new SqlCommand(nomeProcedure, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    foreach (var item in parametros)
                        comando.Parameters.Add(item);

                    await conexao.OpenAsync();
                    await comando.ExecuteNonQueryAsync();
                }
            }
        }

        internal async Task<DataSet> ConsultarAsync(string nomeProcedure, List<SqlParameter> parametros)
        {
            using (SqlConnection conexao = new SqlConnection(stringDeConexao))
            {
                using (SqlCommand comando = new SqlCommand(nomeProcedure, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    foreach (var item in parametros)
                        comando.Parameters.Add(item);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataSet ds = new DataSet();
                        await conexao.OpenAsync();
                        await Task.Run(() => adapter.Fill(ds));
                        return ds;
                    }
                }
            }
        }
    }
}
