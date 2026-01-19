using DesafioApi.Services;
using Xunit;

namespace DesafioApi.Tests.Services;

public class TesteServicoArquivo
{
    private readonly ArquivoService _servico;

    public TesteServicoArquivo()
    {
        _servico = new ArquivoService();
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoCaminhoVazio_LancaArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _servico.LerArquivoAsync(""));
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoCaminhoNulo_LancaArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _servico.LerArquivoAsync(null!));
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoCaminhoApenasEspacos_LancaArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _servico.LerArquivoAsync("   "));
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoArquivoNaoExiste_LancaFileNotFoundException()
    {
        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            _servico.LerArquivoAsync("C:\\arquivo_inexistente.txt"));
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoExtensaoNaoTxt_LancaArgumentException()
    {
        var excecao = await Assert.ThrowsAsync<ArgumentException>(() =>
            _servico.LerArquivoAsync("documento.pdf"));

        Assert.Contains("Apenas arquivos .txt são permitidos", excecao.Message);
    }

    [Theory]
    [InlineData("documento.doc")]
    [InlineData("imagem.png")]
    [InlineData("script.js")]
    [InlineData("dados.json")]
    [InlineData("arquivo.zip")]
    [InlineData("arquivo.exe")]
    public async Task LerArquivoAsync_ComVariasExtensoesNaoTxt_LancaArgumentException(string nomeArquivo)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _servico.LerArquivoAsync(nomeArquivo));
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoArquivoSemExtensao_LancaArgumentException()
    {
        var excecao = await Assert.ThrowsAsync<ArgumentException>(() =>
            _servico.LerArquivoAsync("arquivo_sem_extensao"));

        Assert.Contains("Apenas arquivos .txt são permitidos", excecao.Message);
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoArquivoExiste_RetornaConteudo()
    {
        var diretorioTeste = Path.Combine(Path.GetTempPath(), "TestesDesafioApi");
        Directory.CreateDirectory(diretorioTeste);

        var nomeArquivoTeste = "teste.txt";
        var conteudoTeste = "TESTE";
        var caminhoArquivoTeste = Path.Combine(diretorioTeste, nomeArquivoTeste);

        try
        {
            await File.WriteAllTextAsync(caminhoArquivoTeste, conteudoTeste);

            var resultado = await _servico.LerArquivoAsync(caminhoArquivoTeste);

            Assert.Equal(nomeArquivoTeste, resultado.NomeArquivo);
            Assert.Equal(conteudoTeste, resultado.Conteudo);
        }
        finally
        {
            if (Directory.Exists(diretorioTeste))
                Directory.Delete(diretorioTeste, true);
        }
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoExtensaoTxtMaiuscula_Sucede()
    {
        var diretorioTeste = Path.Combine(Path.GetTempPath(), "TestesDesafioApi");
        Directory.CreateDirectory(diretorioTeste);

        var nomeArquivoTeste = "documento.TXT";
        var conteudoTeste = "TESTE";
        var caminhoArquivoTeste = Path.Combine(diretorioTeste, nomeArquivoTeste);

        try
        {
            await File.WriteAllTextAsync(caminhoArquivoTeste, conteudoTeste);

            var resultado = await _servico.LerArquivoAsync(caminhoArquivoTeste);

            Assert.Equal(nomeArquivoTeste, resultado.NomeArquivo);
            Assert.Equal(conteudoTeste, resultado.Conteudo);
        }
        finally
        {
            if (Directory.Exists(diretorioTeste))
                Directory.Delete(diretorioTeste, true);
        }
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoExtensaoTxtMista_Sucede()
    {
        var diretorioTeste = Path.Combine(Path.GetTempPath(), "TestesDesafioApi");
        Directory.CreateDirectory(diretorioTeste);

        var nomeArquivoTeste = "documento.Txt";
        var conteudoTeste = "TESTE";
        var caminhoArquivoTeste = Path.Combine(diretorioTeste, nomeArquivoTeste);

        try
        {
            await File.WriteAllTextAsync(caminhoArquivoTeste, conteudoTeste);

            var resultado = await _servico.LerArquivoAsync(caminhoArquivoTeste);

            Assert.Equal(nomeArquivoTeste, resultado.NomeArquivo);
            Assert.Equal(conteudoTeste, resultado.Conteudo);
        }
        finally
        {
            if (Directory.Exists(diretorioTeste))
                Directory.Delete(diretorioTeste, true);
        }
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoArquivoVazio_RetornaConteudoVazio()
    {
        var diretorioTeste = Path.Combine(Path.GetTempPath(), "TestesDesafioApi");
        Directory.CreateDirectory(diretorioTeste);

        var nomeArquivoTeste = "vazio.txt";
        var caminhoArquivoTeste = Path.Combine(diretorioTeste, nomeArquivoTeste);

        try
        {
            await File.WriteAllTextAsync(caminhoArquivoTeste, string.Empty);

            var resultado = await _servico.LerArquivoAsync(caminhoArquivoTeste);

            Assert.Equal(nomeArquivoTeste, resultado.NomeArquivo);
            Assert.Empty(resultado.Conteudo);
        }
        finally
        {
            if (Directory.Exists(diretorioTeste))
                Directory.Delete(diretorioTeste, true);
        }
    }

    [Fact]
    public async Task LerArquivoAsync_QuandoArquivoComCaracteresEspeciais_RetornaConteudo()
    {
        var diretorioTeste = Path.Combine(Path.GetTempPath(), "TestesDesafioApi");
        Directory.CreateDirectory(diretorioTeste);

        var nomeArquivoTeste = "especial.txt";
        var conteudoTeste = "áéíóú ñ çã 中文 日本語";
        var caminhoArquivoTeste = Path.Combine(diretorioTeste, nomeArquivoTeste);

        try
        {
            await File.WriteAllTextAsync(caminhoArquivoTeste, conteudoTeste);

            var resultado = await _servico.LerArquivoAsync(caminhoArquivoTeste);

            Assert.Equal(nomeArquivoTeste, resultado.NomeArquivo);
            Assert.Equal(conteudoTeste, resultado.Conteudo);
        }
        finally
        {
            if (Directory.Exists(diretorioTeste))
                Directory.Delete(diretorioTeste, true);
        }
    }
}
