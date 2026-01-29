using DesafioApi.Dtos.Response;

namespace DesafioApi.Services;

public interface IArquivoService
{
    Task<LeituraArquivoResponse> LerArquivoAsync(string caminho);
    Task<LeituraArquivoResponse> AtualizarArquivoAsync(string caminho, string novoConteudo);
}
