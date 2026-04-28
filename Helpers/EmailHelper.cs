using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DeportivoApp.Helpers;

public class EmailHelper
{
    private readonly EmailSettings _settings;

    public EmailHelper(IOptions<EmailSettings> settings)
    {
        // Se inyecta la configuración desde appsettings.json (EmailSettings)
        _settings = settings.Value;
    }

    public async Task EnviarConfirmacionReservaAsync(
        string destinatario,
        string nombreUsuario,
        string nombreEspacio,
        DateTime fechaInicio
    )
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_settings.OverrideRecipientEmail))
            {
                destinatario = _settings.OverrideRecipientEmail;
            }

            // Construcción del correo
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(destinatario));
            message.Subject = "Reserva confirmada";

            var html = $@"
            <html>
                <body style='font-family: Arial; padding:20px;'>
                <h2 style='color:#2c3e50;'>Reserva Confirmada</h2>
                    <p>Hola 👋 <b>{nombreUsuario}</b>,</p>
                    <p>Tu reserva ha sido asignada exitosamente en <b>Deportivo App</b>.</p>
                <hr>
                    <p><b>Espacio:</b> {nombreEspacio}</p>
                    <p><b>📅 Fecha:</b> {fechaInicio}</p>

                <hr>
                    <p style='color:gray;font-size:12px;'>
                    Gracias por confiar en nosotros 💙
                    </p>
            </body>
            </html>";

            message.Body = new BodyBuilder { HtmlBody = html }.ToMessageBody();

            using var smtp = new SmtpClient();

            // Envío real por SMTP usando async and await
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // Manejo de error 
            throw new InvalidOperationException("No se pudo enviar el correo vía SMTP (MailKit).", ex);
        }
    }
}