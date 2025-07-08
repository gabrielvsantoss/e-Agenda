using System.Text.Json.Serialization;
using System.Text.Json;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Dominio.ModuloCategoria;
using e_Agenda.Dominio.ModuloDespesa;
using e_Agenda.Dominio.ModuloTarefas;
using e_Agenda.Dominio.ModuloCompromisso;

namespace e_Agenda.Infraestrura.Arquivos.Compartilhado;


public class ContextoDados
{
    private string pastaArmazenamento = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "eAgenda"
    );
    private string arquivoArmazenamento = "dados.json";

    public List<Contato> Contatos { get; set; }
    public List<Compromisso> Compromissos { get; set; }
    public List<Categoria> Categorias { get; set; }
    public List<Despesa> Despesas { get; set; }
    public List<Tarefa> Tarefas { get; set; }

    public ContextoDados()
    {
        Contatos = new List<Contato>();
        Categorias = new List<Categoria>();
        Despesas = new List<Despesa>();
        Tarefas = new List<Tarefa>();
        Compromissos = new List<Compromisso>();
    }

    public ContextoDados(bool carregarDados) : this()
    {
        if (carregarDados)
            Carregar();
    }

    public void Salvar()
    {
        string caminhoCompleto = Path.Combine(pastaArmazenamento, arquivoArmazenamento);

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        jsonOptions.WriteIndented = true;
        jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

        string json = JsonSerializer.Serialize(this, jsonOptions);

        if (!Directory.Exists(pastaArmazenamento))
            Directory.CreateDirectory(pastaArmazenamento);

        File.WriteAllText(caminhoCompleto, json);
    }

    public void Carregar()
    {
        string caminhoCompleto = Path.Combine(pastaArmazenamento, arquivoArmazenamento);

        if (!File.Exists(caminhoCompleto)) return;

        string json = File.ReadAllText(caminhoCompleto);

        if (string.IsNullOrWhiteSpace(json)) return;

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

        ContextoDados contextoArmazenado = JsonSerializer.Deserialize<ContextoDados>(
            json, 
            jsonOptions
        )!;

        if (contextoArmazenado == null) return;

        Contatos = contextoArmazenado.Contatos;
        Categorias = contextoArmazenado.Categorias;
        Despesas = contextoArmazenado.Despesas;
        Tarefas = contextoArmazenado.Tarefas;
        Compromissos = contextoArmazenado.Compromissos;
        
        RestaurarRelacionamentos();
    }

    private void RestaurarRelacionamentos()
    {
        
        foreach (var categoria in Categorias)
        {
            categoria.Despesas.Clear();
        }

        
        foreach (var despesa in Despesas)
        {
            foreach (var categoriaId in despesa.Categorias.Select(c => c.Id).ToList())
            {
                
                var categoriaReal = Categorias.FirstOrDefault(c => c.Id == categoriaId);
                if (categoriaReal != null)
                {
                    
                    var categoriaIndex = despesa.Categorias.FindIndex(c => c.Id == categoriaId);
                    if (categoriaIndex >= 0)
                    {
                        despesa.Categorias[categoriaIndex] = categoriaReal;
                    }

                    
                    if (!categoriaReal.Despesas.Contains(despesa))
                    {
                        categoriaReal.Despesas.Add(despesa);
                    }
                }
            }
        }
    }
}
