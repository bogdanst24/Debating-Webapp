using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using DatabaseAccess;

namespace DataController.Utility
{
    public static class MailService
    {
        private static readonly string pss = "Sbf240895.";


        internal static void SendVerificationMail(string email, string cod_verificare)
        {
            var link_verificare = "http://dezbateri.ro/verify?email="+email;

            var subject = "dezbateri.ro - E-mail verification";
            var body = "Pentru a iti activa contul creat, e nevoie sa verifici aceasta adresa de E-mail. \r Pentru a verifica E-mailul, introdu codul <b> " + cod_verificare +
                "</b> la urmatorul link <b> " + link_verificare + " </b>.";
            SendMail(email, "registration@dezbateri.ro", subject, body);
        }

        internal static void SendStartDebateMail(User pro_user, User con_user, DebateInfo di)
        {
            var subject = "dezbateri.ro - dezbaterea incepe!";
            var body = "Dezbaterea pe tema " + di.subject + " a fost acceptata de <b>" + con_user.Username + "</b>. Aveti 3 zile pentru a publica primul discurs al dezbaterii.";
            SendMail(pro_user.Email, "contact@dezbateri.ro", subject, body);

            body = "Ai acceptat dezbaterea pe tema " + di.subject + " impotriva lui <b>" + pro_user.Username +
                "</b>. Oponentul are 3 zile pentru a publica primul discurs al dezbaterii, urmand ca apoi apoi sa ai alte 3 zile la dispozitie pentru primul tau discurs.";
            SendMail(con_user.Email, "contact@dezbateri.ro", subject, body);
        }

        internal static void SendNextRoundMail(User pro_username, User con_username, DebateInfo di, int round)
        {
            var subject = "dezbateri.ro - dezbaterea continua!";
            if (round % 2 == 1)
            {
                var body = "Discursul a fost postat de catre <b>" + con_username.Username + "</b> in dezbaterea pe tema " + 
                    di.subject + " intre <b>" + pro_username.Username + "</b> si <b>" + con_username.Username + ". " +
                    "Aveti 3 zile pentru a publica urmatorul discurs al dezbaterii.";
                SendMail(pro_username.Email, "contact@dezbateri.ro", subject, body);

                body = "Discursul tau a fost postat in dezbaterea pe tema " +
                    di.subject + " intre <b>" + pro_username.Username + "</b> si <b>" + con_username.Username + ". " +
                    "Oponentul are 3 zile pentru a publica urmatorul discurs al dezbaterii.";
                SendMail(con_username.Email, "contact@dezbateri.ro", subject, body);
            } else
            {
                var body = "Discursul a fost postat de catre <b>" + pro_username.Username + "</b> in dezbaterea pe tema " +
                    di.subject + " intre <b>" + pro_username.Username + "</b> si <b>" + con_username.Username + ". " +
                    "Aveti 3 zile pentru a publica urmatorul discurs al dezbaterii.";
                SendMail(con_username.Email, "contact@dezbateri.ro", subject, body);

                body = "Discursul tau a fost postat in dezbaterea pe tema " +
                   di.subject + " intre <b>" + pro_username.Username + "</b> si <b>" + con_username.Username + ". " +
                   "Oponentul are 3 zile pentru a publica urmatorul discurs al dezbaterii.";
                SendMail(pro_username.Email, "contact@dezbateri.ro", subject, body);
            }
          
        }

        internal static void SendDoneDebateMail(User pro_username, User con_username, DebateInfo di)
        {
            var subject = "dezbateri.ro - dezbaterea s-a incheiat!";
            var body = "Dezbaterea pe tema " + di.subject + " dintre <b>" + pro_username.Username + "</b> si <b>" + con_username.Username + " s-a incheiat." +
                "Perioada de votare si comentare a dezbaterii incepe!";
            SendMail(pro_username.Email, "contact@dezbateri.ro", subject, body);
            SendMail(con_username.Email, "contact@dezbateri.ro", subject, body);
        }

        private static void SendMail(String email_To, String email_From, String email_Subject, String email_Body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 26;
            client.Host = "mail.dezbateri.ro";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(email_From, pss);
            MailMessage mm = new MailMessage(email_From, email_To, email_Subject, email_Body);
            mm.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            mm.SubjectEncoding = System.Text.Encoding.Default;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mm.IsBodyHtml = true;

            client.Send(mm);
        }

      
    }
}