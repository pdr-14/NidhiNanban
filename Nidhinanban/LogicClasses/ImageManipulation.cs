using System;
using System.IO;
using ImageMagick;

namespace Nidhinanban.LogicClasses
{
    public class ImageManipulation
    {
        public string ConvertBlobToBase64Image(byte[] blobData, string mimeType = "image/png")
        {
            if (blobData == null || blobData.Length == 0)
                throw new ArgumentException("Blob data is null or empty.");

            string base64String = Convert.ToBase64String(blobData);
            return $"data:{mimeType};base64,{base64String}";
        }


        public  byte[] ConvertBase64ImageToBlob(string base64Data)
        {
            if (string.IsNullOrWhiteSpace(base64Data))
            {
                return null;
            }
                //throw new ArgumentException("Input Base64 string is null or empty.");

            // Check if the string contains the metadata prefix
            int commaIndex = base64Data.IndexOf(',');

            string base64String = (commaIndex >= 0) ? base64Data.Substring(commaIndex + 1) : base64Data;

            // Convert Base64 string to byte array
            return Convert.FromBase64String(base64String);
        }

        public async Task StoreImageToTheServer(string CustomerID, IFormFile ImageData, string type)
        {
                using var memoryStream = new MemoryStream();
                await ImageData.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
            using var image = new MagickImage(memoryStream);
            if (Directory.Exists("Customer_Images"))
            {
                string path = Path.Combine("Customer_Images", CustomerID);
                Directory.CreateDirectory(path);
                var filename = CustomerID + type + "Image.jpg";
                var filepath = Path.Combine(path, filename);
                if(type=="Profile")
                {
                    image.Resize(400,400);
                }
                image.Quality = 75;
                await image.WriteAsync(filepath);
            }
            else
            {
                Directory.CreateDirectory("Customer_Images");
                string path = Path.Combine("Customer_Images", CustomerID);
                Directory.CreateDirectory(path);
                var filename = CustomerID + type + "Image.jpg";
                var filepath = Path.Combine(path, filename);
                if(type=="Profile")
                {
                    image.Resize(400,400);
                }
                image.Quality = 75;
                await image.WriteAsync(filepath);
            }
        }
    }
}