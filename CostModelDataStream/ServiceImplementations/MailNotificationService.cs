using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CostModelDataStream.ServiceImplementations
{
   public class MailNotificationService
    {
        public static void SendMail(string templateName, Dictionary<string, string> replacements)
        {
            //MailPriority priority = MailPriority.Normal, bool writeAsFile = false
            string @sendTo = ConfigurationManager.AppSettings["sendTo"];

            MailDefinition md = new MailDefinition();
            md.BodyFileName = templateName;
            md.From = ConfigurationManager.AppSettings["SmtpFromMail"];
            md.Subject = "MCQ Feed File Process Notification";
            md.IsBodyHtml = true;
            MailMessage msg = md.CreateMailMessage(@sendTo, null, new System.Web.UI.Control());
            foreach (var r in replacements)
            {
                string placeholder = String.Format(@"<%{0}%>", r.Key);
                msg.Body = msg.Body.Replace(placeholder, r.Value);
            }
            SendEmail(msg.Body);
        }

        public static void SendEmail(string body)
        {
            string SmtpClientIP = ConfigurationManager.AppSettings["SmtpClientIP"];
            int SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]);
            string SmtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            string SmtpFromMail = ConfigurationManager.AppSettings["SmtpFromMail"];
            SmtpClient smtout = new SmtpClient(SmtpClientIP, SmtpClientPort);
            smtout.EnableSsl = true;
            smtout.Credentials = new System.Net.NetworkCredential(SmtpUser, SmtpPassword);
            MailMessage email = new MailMessage();
            MailAddress froma = new MailAddress(SmtpFromMail);
            email.From = froma;
            email.To.Add(ConfigurationManager.AppSettings["sendTo"]);
            email.CC.Add(ConfigurationManager.AppSettings["sendToCC"]);
            email.Subject = "Cost Model File Process Notification";
            email.IsBodyHtml = true;
            email.Body = body;
            try
            {
                smtout.Send(email);
            }
            catch (SmtpException ex)
            {
                CostModelLogger.ErrorLogger($"Error Occured While Sending Mail, {ex.Message}");  
            }
        }
    }
}
