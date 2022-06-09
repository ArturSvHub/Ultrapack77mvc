using MailKit.Net.Smtp;

using MimeKit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UpakUtilitiesLibrary;
using UpakUtilitiesLibrary.Utility.EmailServices;

namespace UpakModelsLibrary.Models.ViewModels
{
	public class MessageSenderVM
	{
		public string? Email { get; set; }
		public string? Subject { get; set; }
		public string? Message { get; set; }
		public async Task SendMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            var settings = new Settings();
            settings.EmailSettings.FromTitle = "Заказ Upak77";

            emailMessage.From.Add(new MailboxAddress(settings.EmailSettings.FromTitle, settings.EmailSettings.FromEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(settings.EmailSettings.SmtpClientAdress, 25, false);
                await client.AuthenticateAsync(settings.EmailSettings.AuthLogin, settings.EmailSettings.AuthPass);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
