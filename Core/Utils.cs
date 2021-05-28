using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace gestion.site.Core
{
    public static class RolAcceso
    {
        public const string ADMIN = "ADMIN";
        public const string USER = "USER";
        public const string DIRECTOR = "DIRECTOR";
        public const string SUPER = "SUPER";
    }

    public class Utils
    {

        public static string CifrarTexto(string texto)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] bytes = encoding.GetBytes(texto);
            SHA512Managed sha512 = new SHA512Managed();
            return encoding.GetString(sha512.ComputeHash(bytes));
        }

        public static bool ValidarTexto(string texto, string textoCifrado)
        {
            string valorCifrado = CifrarTexto(texto);
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] bytesIngreso = encoding.GetBytes(valorCifrado);
            byte[] bytesReferencia = encoding.GetBytes(textoCifrado);
            return ValidarBytes(bytesIngreso, bytesReferencia);
        }

        private static bool ValidarBytes(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1.Length != bytes2.Length)
            {
                return false;
            }
            for (int i = 0; i < bytes1.Length; i++)
            {
                if (!(bytes1[i].Equals(bytes2[i])))
                {
                    return false;
                }
            }
            return true;
        }

        public static void EnviarCorreo(string destinatario,  string asunto, string mensaje, bool html)
        {
            SmtpClient cliente=new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("umg.proyecto.final@gmail.com", "75315900")
            };

            MailMessage email = new MailMessage("umg.proyecto.final@gmail.com", destinatario, asunto, mensaje);
            email.IsBodyHtml = html;
            cliente.Send(email);
        }
    }
}
