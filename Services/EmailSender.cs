using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace CorporateAssetManager.Services;

/// <summary>
/// Implementación mock de IEmailSender para desarrollo.
/// En producción, reemplazar con una implementación real (SendGrid, SMTP, etc.)
/// </summary>
public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // En desarrollo, solo registramos el email en los logs
        // En producción, aquí iría la lógica real de envío de emails
        _logger.LogInformation("Email enviado a: {Email}, Asunto: {Subject}", email, subject);
        _logger.LogDebug("Contenido del email: {Message}", htmlMessage);
        
        // Retornamos Task.CompletedTask porque no enviamos realmente el email
        return Task.CompletedTask;
    }
}
