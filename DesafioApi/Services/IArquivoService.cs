using DesafioApi.Dtos.Response;

namespace DesafioApi.Services;

public interface IArquivoService
{
    Task<LeituraArquivoResponse> LerArquivoAsync(string caminho);
    Task AtualizarArquivoAsync(string caminho, string novoConteudo);
}
