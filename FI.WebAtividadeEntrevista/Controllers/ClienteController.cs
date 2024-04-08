using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Servicos;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FI.AtividadeEntrevista.DAL;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Incluir(ClienteModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> erros = ModelState.Values.SelectMany(item => item.Errors).Select(error => error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }
                else
                {
                    BoCliente bo = new BoCliente();

                    model.Id = await bo.IncluirAsync(new Cliente()
                    {
                        CEP = model.CEP,
                        Cidade = model.Cidade,
                        Email = model.Email,
                        Estado = model.Estado,
                        Logradouro = model.Logradouro,
                        Nacionalidade = model.Nacionalidade,
                        Nome = model.Nome,
                        Sobrenome = model.Sobrenome,
                        Telefone = model.Telefone,
                        CPF = model.CPF
                    });

                    if (model.Beneficiarios != null && model.Beneficiarios.Count > 0)
                    {
                        BoBeneficiario boBeneficiario = new BoBeneficiario();
                        foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                        {
                            Beneficiario ben = new Beneficiario();
                            ben.Nome = beneficiario.Nome;
                            ben.CPF = beneficiario.CPF;
                            ben.IdCliente = model.Id;
                            await boBeneficiario.IncluirAsync(ben);
                        }
                    }

                    return Json("Cadastro efetuado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                return Json($"Ops, ocorreu um erro no servidor! {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Alterar(ClienteModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> erros = ModelState.Values.SelectMany(item => item.Errors).Select(error => error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }
                else
                {
                    BoCliente bo = new BoCliente();

                    await bo.AlterarAsync(new Cliente()
                    {
                        Id = model.Id,
                        CEP = model.CEP,
                        Cidade = model.Cidade,
                        Email = model.Email,
                        Estado = model.Estado,
                        Logradouro = model.Logradouro,
                        Nacionalidade = model.Nacionalidade,
                        Nome = model.Nome,
                        Sobrenome = model.Sobrenome,
                        Telefone = model.Telefone,
                        CPF = model.CPF
                    });

                    if (model.Beneficiarios.Count > 0)
                    {
                        BoBeneficiario boBeneficiario = new BoBeneficiario();
                        foreach (BeneficiarioModel beneficiarioModel in model.Beneficiarios)
                        {
                            Beneficiario ben = new Beneficiario();
                            ben.Id = beneficiarioModel.Id;
                            ben.Nome = beneficiarioModel.Nome;
                            ben.CPF = beneficiarioModel.CPF;
                            ben.IdCliente = model.Id;

                            if (beneficiarioModel.isSaved && beneficiarioModel.changed)
                                await boBeneficiario.AlterarAsync(ben);
                            else if (beneficiarioModel.isSaved == false)
                                await boBeneficiario.IncluirAsync(ben);
                            else if (beneficiarioModel.isSaved && beneficiarioModel.isDeleted)
                                await boBeneficiario.ExcluirAsync(beneficiarioModel.Id);
                        }
                    }

                    return Json("Cadastro alterado com sucesso");
                }
            }
            catch (Exception ex) 
            {
                return Json($"Ops, ocorreu um erro no servidor! {ex.Message}");
            }       
        }

        [HttpGet]
        public async Task<ActionResult> Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = await bo.ConsultarAsync(id);
            ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = new List<BeneficiarioModel>()
                };

                BoBeneficiario boBeneficiario = new BoBeneficiario();
                List<Beneficiario> beneficiarios = await boBeneficiario.ListarPorClienteAsync(id);

                if (beneficiarios.Count > 0)
                {
                    foreach (Beneficiario b in beneficiarios)
                    {
                        model.Beneficiarios.Add(new BeneficiarioModel
                        {
                            Id = b.Id,
                            Nome = b.Nome,
                            CPF = b.CPF,
                            isSaved = true,
                            isDeleted = false,
                            changed = false,
                        });
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                listarCliente parametros = await new BoCliente().PesquisaAsync(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase));

                //Return result to jTable
                return Json(new { Result = "OK", Records = parametros.clientes, TotalRecordCount = parametros.qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}