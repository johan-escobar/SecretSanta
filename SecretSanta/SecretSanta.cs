using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using ConfigR;


namespace SecretSanta
{

    public class SecretSanta : ISecretSanta
    {
        MyConfig data =
            new Config().UseRoslynCSharpLoader("settings\\secretsanta.csx")
            .Load<MyConfig>()
            .GetAwaiter()
            .GetResult();
        public void SendEmail(Participant secretSanta, string to)
        {

            string body =
                "<img src = \"https://media.giphy.com/media/NZmrvsfxpbTSU/giphy.gif\" height = \"150\" width = \"260\" >" +
                "<br>" +
                "<br>" +
                "<font size = \"6\" > " + char.ToUpper(secretSanta.Name[0]) + secretSanta.Name.Substring(1) + ", has sido seleccionado como el amigo secreto de <b>" + char.ToUpper(to[0]) + to.Substring(1) + "</b></font> " +
                "<br>" +
                "<br>" +
                "<font size = \"6\" >La cena y el intercambio de regalos tomaran lugar en la casa de Danny el 24 de Diciembre (la hora no es aun comfirmada).</font> " +
                "<br>" +
                "<br>" +
                "<font size = \"6\" ><b><u>£20 es el presupuesto para cada regalo</u></b></font> " +
                "<br>" +
                "<br>" +
                "<font size = \"6\" >Trae tu regalo embuelto con el nombre de la persona a la cual le pertenece el regalo en un lugar claramente visible.</font>" +
                "<br>" +
                "<br>" +
                "</font><br><img src = \"https://media.giphy.com/media/HUgleuD34CGVq/giphy.gif\" height = \"150\" width = \"260\" >" +
                "<br> " +
                "<br>" +
                "<font size = \"6\" >Para mantener la magia de la navidad, asegurate de mantener tu identidad en secreto durante la noche y por el resto de la eternidad. </font>" +
                "<br>" +
                "<br>" +
                "</font><br><img src = \"https://media.giphy.com/media/yqWb3RwHt1tYs/giphy.gif\" height = \"150\" width = \"260\" >" +
                "<br>" +
                "<br>" +
                "<font size=\"6\" >Recuerda que reemplazando Santa!</font>" +
                "<br> " +
                "<br>" +
                "<img src=\"https://media.giphy.com/media/3o6ZtpBTZZ6AaKHXwc/source.gif\" height=\"150\" width=\"260\">" +
                "<br>" +
                "<br>";


            SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            var mail = new MailMessage();
            mail.From = new MailAddress(data.EmailAccount);
            mail.To.Add(secretSanta.Email);
            mail.Subject = "Secret santa (Randomly Generated)";
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = body;
            mail.Body = htmlBody;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(data.EmailAccount, data.Password);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
            SmtpServer.Dispose();
            
            
        }

        public List<Participant> SecretSantaList(string participantsData)
        {
            var participantsList = new List<Participant>();

            var gg = participantsData.Split(';');
            foreach (var g in gg)
            {
                var t = g.Split(',');
                participantsList.Add(new Participant() { Name = t[0].ToLower(), Email = t[1].ToLower(), Partner = t[2].ToLower() });
            }

            return participantsList;

        }


        public void RandomNameSelector()
        {
            List<Participant> participants = new SecretSanta().SecretSantaList(data.ParticipantCsvData);

            var secretSantaList = new Dictionary<string, string>();
        resrtart:
            foreach (var participant in participants)
            {
                var azar = new Random();
                var giftReciever = participants.ElementAt(azar.Next(0, participants.Count));
                int iterations = 0;
                while (secretSantaList.ContainsKey(giftReciever.Name) || true && participant.Name == giftReciever.Name || giftReciever.Name == participant.Partner)
                {
                    giftReciever = participants.ElementAt(azar.Next(0, participants.Count));
                    iterations++;
                    if (iterations > 9) break;
                }
                try
                {
                    secretSantaList.Add(giftReciever.Name, participant.Name);
                    new SecretSanta().SendEmail(participant, giftReciever.Name);
                }
                catch (Exception)
                {
                    secretSantaList.Clear();
                    goto resrtart;
                }


            }

        }

    }

}
