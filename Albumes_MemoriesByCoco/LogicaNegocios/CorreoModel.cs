using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Albumes_MemoriesByCoco.LogicaNegocios
{
    public class CorreoModel
    {

       
            SmtpClient smtp = new SmtpClient();
            public void ConfiguracionCorreo()
            {
                try
                {

                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("pruebasmemories@gmail.com", "zcaxgzejezqpzjiv");
                }
                catch (Exception ex)
                {

                }

            }
            public async Task EnviarCorreo(string correo, string asunto, string cuerpo)
            {
                try
                {
               
                string footerCuerpo = "<hr> <p>Memories By Coco " + DateTime.Now.Year + "</p>";
                cuerpo = cuerpo+footerCuerpo;
                await Task.Run(() =>
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("pruebasmemories@gmail.com", "Kindred1595");
                    MailMessage mm = new MailMessage();
                    mm.IsBodyHtml = true;
                    mm.Priority = MailPriority.Normal;
                    mm.From = new MailAddress(correo);
                    mm.Subject = asunto;
                    mm.Body = cuerpo;

                    mm.To.Add(new MailAddress(correo));
                    smtp.Send(mm);

                    Task.Delay(100).Wait();
                });
              
                }
                catch (Exception ex)
                {

                }
            }


        }
}