using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using e_Agenda.Dominio.ModuloCompromisso;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace e_Agenda.ConsoleApp.Models
{
    public abstract class FormularioCompromissoViewModel
    {

        [Required(ErrorMessage = "O Campo titulo é obrigatório")]
        [MinLength(3, ErrorMessage = "O Campo titulo deve conter ao minimo 3 caracteres")]
        [MaxLength(100, ErrorMessage = "O Campo titulo deve conter ao maximo 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O Campo assunto é obrigatório")]
        [MinLength(3, ErrorMessage = "O Campo assunto deve conter ao minimo 3 caracteres")]
        [MaxLength(100, ErrorMessage = "O Campo assunto deve conter ao maximo 100 caracteres")]
        public string Assunto { get; set; }

        public DateTime DataOcorrencia { get; set; }

        [Required(ErrorMessage = "O Campo Hora de Inico é obrigatório")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "O Campo Hora de Termino é obrigatório")]
        public TimeSpan HoraTermino { get; set; }
        public string TipoCompromisso { get; set; }
        public string Local { get; set; }
        public string Link { get; set; }
        public Guid? ContatoId { get; set; }

        public List<SelecionarContatoViewModel> ContatosDisponiveis { get; set; }

        protected FormularioCompromissoViewModel()
        {
            ContatosDisponiveis = new List<SelecionarContatoViewModel>();
        }
    }

    public class SelecionarContatoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public SelecionarContatoViewModel(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
    public class CadastrarCompromissoViewModel : FormularioCompromissoViewModel
    {
        public CadastrarCompromissoViewModel()
        {

        }
        public CadastrarCompromissoViewModel(List<Contato> contatos) : this()
        {
            foreach (Contato contato in contatos)
            {
                var contatoVM = new SelecionarContatoViewModel(contato.Id, contato.Nome);
                ContatosDisponiveis.Add(contatoVM);
            }
        }
    }

    public class VisualizarCompromissosViewModel
    {
        public List<DetalhesCompromissoViewModel> Registros { get; set; }

        public VisualizarCompromissosViewModel(List<Compromisso> compromissos)
        {
            Registros = new List<DetalhesCompromissoViewModel>();

            foreach (Compromisso m in compromissos)
            {
                DetalhesCompromissoViewModel detalheVM = m.ParaDetalheVM();

                Registros.Add(detalheVM);
            }
        }
    }
    public class EditarCompromissoViewModel : FormularioCompromissoViewModel
    {
        public Guid Id { get; set; }
        public EditarCompromissoViewModel()
        {

        }
        public EditarCompromissoViewModel(Guid id, string titulo, string assunto,  DateTime dataOcorrencia, TimeSpan horaInicio, TimeSpan horaTermino, string tipoCompromisso, string local, string link, Guid contatoId)
        {
            Id = id;
            Titulo = titulo;
            Assunto = assunto;
            DataOcorrencia = dataOcorrencia;
            HoraInicio = horaInicio;
            HoraTermino = horaTermino;
            TipoCompromisso = tipoCompromisso;
            Local = local;
            Link = link;
            ContatoId = contatoId;
        }
        public EditarCompromissoViewModel(Guid id, string titulo, string assunto, DateTime dataOcorrencia, TimeSpan horaInicio, TimeSpan horaTermino, string tipoCompromisso, string local, string link)
        {
            Id = id;
            Titulo = titulo;
            Assunto = assunto;
            DataOcorrencia = dataOcorrencia;
            HoraInicio = horaInicio;
            HoraTermino = horaTermino;
            TipoCompromisso = tipoCompromisso;
            Local = local;
            Link = link;
        }
    }

    public class ExcluirCompromissoViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }

        public ExcluirCompromissoViewModel(Guid id, string titulo)
        {
            Id = id;
            Titulo = titulo;
        }
    }

    public class DetalhesCompromissoViewModel
    {
            public Guid Id { get; set; }
            public string Titulo { get; set; }
            public string Assunto { get; set; }
            public DateTime DataOcorrencia { get; set; }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraTermino { get; set; }
            public string TipoCompromisso { get; set; }
            public string Local { get; set; }
            public string Link { get; set; }
            public Guid ContatoId { get; set; }
            public string NomeContato { get; set; }
        
        public DetalhesCompromissoViewModel(Guid id, string titulo, string assunto, DateTime dataOcorrencia, TimeSpan horaInicio, TimeSpan horaTermino, string tipoCompromisso, string local, string link, Guid contatoId)
        {
            Id = id;
            Titulo = titulo;
            Assunto = assunto;
            DataOcorrencia = dataOcorrencia;
            HoraInicio = horaInicio;
            HoraTermino = horaTermino;
            TipoCompromisso = tipoCompromisso;
            Local = local;
            Link = link;
            ContatoId = contatoId;
        }
        public DetalhesCompromissoViewModel(Guid id, string titulo, string assunto, DateTime dataOcorrencia, TimeSpan horaInicio, TimeSpan horaTermino, string tipoCompromisso, string local, string link) 
        {
            Id = id;
            Titulo = titulo;
            Assunto = assunto;
            DataOcorrencia = dataOcorrencia;
            HoraInicio = horaInicio;
            HoraTermino = horaTermino;
            TipoCompromisso = tipoCompromisso;
            Local = local;
            Link = link;
        }
    }
}