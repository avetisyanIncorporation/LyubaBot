using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Lyuba02_with_luis_ai
{
    [Serializable]
    public class MailSender
    {
        
        public async Task SendCode(string eemail, string code)
        {

            MailAddress from = new MailAddress("super.geroi2018@yandex.ru", "BotEvgen");
            MailAddress to = new MailAddress(eemail);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Редактирование электронной почты";
            m.Body = code;
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
            smtp.Credentials = new NetworkCredential("super.geroi2018@yandex.ru", "1234Qwer");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }

    }
}