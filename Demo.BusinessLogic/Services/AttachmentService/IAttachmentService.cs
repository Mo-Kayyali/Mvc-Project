using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.AttachmentService
{
    public interface IAttachmentService
    {
        bool Delete(string fileName);

        string? Upload(IFormFile file,string folderName);
    }
}
