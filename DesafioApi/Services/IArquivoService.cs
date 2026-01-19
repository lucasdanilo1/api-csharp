using DesafioApi.Dtos;

namespace DesafioApi.Services;

public interface IArquivoService
{
    Task<TextoLeituraArquivoDto> LerArquivoAsync(string caminho);
}
