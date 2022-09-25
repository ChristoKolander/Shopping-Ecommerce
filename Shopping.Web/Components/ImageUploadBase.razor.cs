using Microsoft.AspNetCore.Components;
using Shopping.Web.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using BlazorInputFile;
using System.Linq;

namespace Shopping.Web.Components
{
    public class ImageUploadBase : ComponentBase
    {

        public string status;

        [Parameter]
        public string ImgUrl { get; set; }

        [Parameter]
        public EventCallback<string> OnChange { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        public async Task HandleSelected(IFileListEntry[] files)

        {

            var file = files.FirstOrDefault();
            if (file != null)
            {
                var memoryStream = new MemoryStream();
                await file.Data.CopyToAsync(memoryStream);

                status = $"Upload {file.Size} bytes from {file.Name}";
                var content = new MultipartFormDataContent
                {
                    { new ByteArrayContent(memoryStream.GetBuffer()), "\"uploadFiles\"", file.Name }
                };
                await ProductService.UploadProductImage(content);
            }
        }

    }
}
