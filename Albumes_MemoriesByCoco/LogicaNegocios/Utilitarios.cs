using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Albumes_MemoriesByCoco.LogicaNegocios
{
    public class Utilitarios
    {
        public enum eTipoOperacion
        {
            Insertar = 1,
            Eliminar = 2,
            Actualizar = 3,
            EliminarReferencias=6

        }

        public enum eTipoActualizaInfo
        {
            Configuracion=1,
            Envio=2
        }
        public bool PasswordValida(string password, string password2)
        {
            bool respuesta = false;
            try
            {
                if (password == password2)
                {
                    string path = @"(?=(.*[a-z]))(?=(.*[A-Z]))(?=(.*\d))(?=(.*[ !""#$%&'()*+,-./:;<=>?@\[\]\^_`{|}~]))^.{8,32}$";
                    Regex rgx = new Regex(path);
                    if (rgx.IsMatch(password))
                    {
                        respuesta = true;
                    }
                }
                
            }
            catch(Exception ex)
            {

            }
            return respuesta;
        }
        public byte[] ConvertirStringToByte(string strImagen)
        {
            byte[] byteArray=null;
            try
            {
                
                 byteArray = Encoding.ASCII.GetBytes(strImagen);

              
            }
            catch(Exception ex)
            {

            }
            return byteArray;
        }


        public  byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        // Resize a Bitmap  
        private Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }



    }
}