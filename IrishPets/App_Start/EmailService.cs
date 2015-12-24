using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IrishPets
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage _message)
        {
            // Login credentials of the sender (me)
            var __from = Properties.Resources.DefEmail_Account;
            var __pass = Properties.Resources.DefEmail_Pass; // old #Valsky01

            // address and smtp server which is used to send the e-mail
            var __client = new SmtpClient("smtp.gmail.com", 587);

            __client.Credentials = new NetworkCredential(__from, __pass);
            __client.EnableSsl = true;

            // E-mail creation: message.Destination - address of the receiver
            var __mail = new MailMessage(__from, _message.Destination);
            __mail.Subject = _message.Subject;
            __mail.Body = _message.Body;
            __mail.IsBodyHtml = true;

            return __client.SendMailAsync(__mail);
        }
    }
}