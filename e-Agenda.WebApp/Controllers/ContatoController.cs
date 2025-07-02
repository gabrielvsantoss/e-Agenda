
using Microsoft.AspNetCore.Mvc;
using e_Agenda.WebApp.Extensions;
using e_Agenda.Infraestrura.Arquivos.Compartilhado;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Infraestrura.Arquivos.ModuloContato;
using e_Agenda.WebApp.Models;
using eAgenda.Infraestrutura.SqlServer;

namespace e_Agenda.WebApp.Controllers
{
    [Route("contatos")]
    public class ContatoController : Controller
    {
        private readonly IRepositorioContato repositorioContato;
        public ContatoController()
        {
            repositorioContato = new RepositorioContatoEmSql();
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var registros = repositorioContato.SelecionarRegistros();

            var visualizarVM = new VisualizarContatosViewModel(registros);

            return View(visualizarVM);
        }


        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            var cadastrarVM = new CadastrarContatoViewModel();

            return View(cadastrarVM);
        }


        [HttpPost("cadastrar")]
        public IActionResult Cadastrar(CadastrarContatoViewModel cadastrarVM)
        {
            var registros = repositorioContato.SelecionarRegistros();

            
                foreach (var item in registros)
                {
                    if (item.Telefone == cadastrarVM.Telefone)
                    {
                        ModelState.AddModelError("Cadastro Unico", "Já existe um contato cadastrado com esse telefone");
                        break;
                    }


                    if (item.Email == cadastrarVM.Email)
                    {
                        ModelState.AddModelError(nameof(cadastrarVM.Email), "Já existe um contato cadastrado com esse email.");
                        break;
                    }
                }

                if (!ModelState.IsValid)
                {
                    return View(cadastrarVM);
                }
            
            var entidade = cadastrarVM.ParaEntidade();

            repositorioContato.CadastrarRegistro(entidade);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("editar/{id:guid}")]
        public IActionResult Editar(Guid id)
        {
            var registroSelecionado = repositorioContato.SelecionarRegistroPorId(id);
            var editarVM = new EditarContatoViewModel(id, registroSelecionado.Nome, registroSelecionado.Email,registroSelecionado.Telefone, registroSelecionado.Cargo, registroSelecionado.Empresa);

             return View(editarVM);
        }


        [HttpPost("editar/{id:guid}")]
        public IActionResult Editar(Guid id, EditarContatoViewModel editarVM)
        {
            var registros = repositorioContato.SelecionarRegistros();

            foreach (var item in registros)
            {
                if (item.Id != id && item.Telefone == editarVM.Telefone)
                {
                    ModelState.AddModelError("Cadastro Unico", "Já existe um contato cadastrado com esse telefone");
                    break;
                }


                if (item.Id != id && item.Email == editarVM.Email)
                {
                    ModelState.AddModelError(nameof(editarVM.Email), "Já existe um contato cadastrado com esse email.");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(editarVM);
            }
            var entidadeEditada = editarVM.ParaEntidade();

            repositorioContato.EditarRegistro(id, entidadeEditada);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("excluir/{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var registroSelecionado = repositorioContato.SelecionarRegistroPorId(id);

            var excluirVM = new ExcluirContatoViewModel(registroSelecionado.Id, registroSelecionado.Nome);

            return View(excluirVM);
        }

        [HttpPost("excluir/{id:guid}")]
        public ActionResult ExcluirConfirmado(Guid id)
        {
            repositorioContato.ExcluirRegistro(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("detalhes/{id:guid}")]
        public ActionResult Detalhes(Guid id)
        {
            var registroSelecionado = repositorioContato.SelecionarRegistroPorId(id);

            var detalhesVM = new DetalhesContatoViewModel(
                id,
                registroSelecionado.Nome,
                registroSelecionado.Email,
                registroSelecionado.Telefone,
                registroSelecionado.Cargo,
                registroSelecionado.Empresa
            );

            return View(detalhesVM);
        }
    }
}

