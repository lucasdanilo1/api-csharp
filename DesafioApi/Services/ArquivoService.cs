using System.Security;
using DesafioApi.Dtos.Response;

namespace DesafioApi.Services;

public class ArquivoService : IArquivoService
{
    public async Task<LeituraArquivoResponse> LerArquivoAsync(string caminho)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caminho);

        var caminhoCompleto = Path.GetFullPath(caminho);

        var extensaoArquivo = Path.GetExtension(caminhoCompleto);

        if (!extensaoArquivo.Equals(".txt", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Apenas arquivos .txt são permitidos.");

        if (!File.Exists(caminhoCompleto))
            throw new FileNotFoundException("Arquivo não encontrado.");

        try
        {
            var conteudo = await File.ReadAllTextAsync(caminhoCompleto);
            return new LeituraArquivoResponse(Path.GetFileName(caminhoCompleto), conteudo);
        }
        catch (Exception ex) when (ex is IOException ||
                                   ex is SecurityException ||
                                   ex is UnauthorizedAccessException)
        {
            throw new InvalidOperationException("Erro ao ler o arquivo.", ex);
        }
    }

    public async Task AtualizarArquivoAsync(string caminho, string novoConteudo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caminho);

        var caminhoCompleto = Path.GetFullPath(caminho);

        var extensaoArquivo = Path.GetExtension(caminhoCompleto);

        if (!extensaoArquivo.Equals(".txt", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Apenas arquivos .txt são permitidos.");

        if (!File.Exists(caminhoCompleto))
            throw new FileNotFoundException("Arquivo não encontrado.");

        try
        {
            await File.WriteAllTextAsync(caminhoCompleto, novoConteudo);
        }
        catch (Exception ex) when (ex is IOException ||
                                   ex is SecurityException ||
                                   ex is UnauthorizedAccessException)
        {
            throw new InvalidOperationException("Erro ao atualizar o arquivo.", ex);
        }
    }
}
