using Microsoft.AspNetCore.Components;
using Shopping.Web.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using BlazorInputFile;
using System.Linq;
using Tewr.Blazor.FileReader;

namespace Shopping.Web.Components
{
    public class ImageUploadBase : ComponentBase
    {

        public ElementReference input;

        [Parameter]
        public string ImgUrl { get; set; }

        [Parameter]
        public EventCallback<string> OnChange { get; set; }

        [Inject]
        public IFileReaderService FileReaderService { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        public async Task HandleSelected()
        {
            foreach (var file in await FileReaderService.CreateReference(input).EnumerateFilesAsync())
            {
                if (file != null)
                {
                    var fileInfo = await file.ReadFileInfoAsync();
                    using (var memoryStream = await file.CreateMemoryStreamAsync(4 * 1024))
                    {
                        var content = new MultipartFormDataContent();
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                        content.Add(new StreamContent(memoryStream, Convert.ToInt32(memoryStream.Length)), "image", fileInfo.Name);
                        ImgUrl = await ProductService.UploadProductImage(content);
                        await OnChange.InvokeAsync(ImgUrl);
                    }
                }
            }

        }
    }
}
