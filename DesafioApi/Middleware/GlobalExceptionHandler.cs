using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace DesafioApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var respostaErro = exception switch
        {
            JsonException jsonEx when jsonEx.Message.Contains("invalid escapable character", StringComparison.OrdinalIgnoreCase)
                => new ErrorResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Erro no formato JSON: Path do arquivo detectado com barras invertidas não escapadas. " +
                    "Use barras duplas (\\) para caminhos do Windows. Ou use barras normais ao invés das barras invertidas "),
            JsonException => new ErrorResponse((int)HttpStatusCode.BadRequest, "Erro no formato JSON: verifique se o corpo da requisição está correto."),
            ArgumentException => new ErrorResponse((int)HttpStatusCode.BadRequest, exception.Message),
            UnauthorizedAccessException => new ErrorResponse((int)HttpStatusCode.Forbidden, "Acesso negado."),
            KeyNotFoundException => new ErrorResponse((int)HttpStatusCode.NotFound, exception.Message),
            InvalidOperationException => new ErrorResponse((int)HttpStatusCode.BadRequest, exception.Message),
            _ => new ErrorResponse((int)HttpStatusCode.InternalServerError, "Um erro desconhecido ocorreu.")
        };

        context.Response.StatusCode = respostaErro.CodigoStatus;
        await context.Response.WriteAsJsonAsync(respostaErro, cancellationToken);

        return true;
    }
}

public record ErrorResponse(int CodigoStatus, string Mensagem);
