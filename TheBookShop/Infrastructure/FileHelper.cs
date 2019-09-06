using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace TheBookShop.Infrastructure
{
    public static class FileHelper
    {
        private static string WebRootPath { get;} = Path.Combine(Environment.CurrentDirectory, "wwwroot");

        public static void CopyImageFile(IFormFile file)
        {
            var uploads = Path.Combine(WebRootPath, "Images\\");
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                   
                }

            }
        }
    }
}