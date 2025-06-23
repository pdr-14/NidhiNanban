using System;
using System.IO;
using ImageMagick;

namespace Nidhinanban.LogicClasses
{
    public class ImageManipulation
    {
        private readonly IWebHostEnvironment _env;
        public ImageManipulation(IWebHostEnvironment environment)
        {
            _env = environment;
        }
    
        public string ConvertBlobToBase64Image(byte[] blobData, string mimeType = "image/png")
        {
            if (blobData == null || blobData.Length == 0)
                throw new ArgumentException("Blob data is null or empty.");

            string base64String = Convert.ToBase64String(blobData);
            return $"data:{mimeType};base64,{base64String}";
        }


        public async Task StoreImageToTheServer(string CustomerID, IFormFile ImageData, string type)
        {
            Console.WriteLine("In the Store Image Function");
                using var memoryStream = new MemoryStream();
                await ImageData.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
            using var image = new MagickImage(memoryStream);
            string folderPath = Path.Combine(_env.ContentRootPath, "Customer_Images");
            Console.WriteLine(folderPath);
            if (Directory.Exists(folderPath))
            {

                string path = Path.Combine(folderPath, CustomerID);
                Console.WriteLine(path);
                Directory.CreateDirectory(path);
                var filename = CustomerID + type + "Image.jpg";
                var filepath = Path.Combine(path, filename);
                if (type == "Profile")
                {
                    image.Resize(400, 400);
                }
                else
                {
                    image.Resize(1280, 720);
                }
                image.Quality = 80;
                await image.WriteAsync(filepath);
            }
            else
            {
                string path = Path.Combine(folderPath, CustomerID);
                Console.WriteLine(path);
                Directory.CreateDirectory(path);
                var filename = CustomerID + type + "Image.jpg";
                var filepath = Path.Combine(path, filename);
                if (type == "Profile")
                {
                    image.Resize(400, 400);
                }
                image.Quality = 75;
                await image.WriteAsync(filepath);
            }
        }
    }
}