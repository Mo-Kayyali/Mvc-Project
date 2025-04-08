using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private List<string> _allowedExtenstions = [".png" ,".jpeg",".jpg"];
        private const int MaxSize = 2097152;
        public string? Upload(IFormFile file, string folderName)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_allowedExtenstions.Contains(extension))
            {
                return null;
            }
            if (file.Length > MaxSize)
            {
                return null;
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath=Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath,FileMode.Create);
            file.CopyTo(stream);
            return fileName;


        }

        public bool Delete(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;
            }
            File.Delete(fileName);
            return true;
        }

    }
}
