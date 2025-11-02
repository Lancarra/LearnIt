using Domain.Models.Attachment;
using Infrastructure.BlobStorage.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Blob;

[Authorize]
[Route("api/static-icons")]
[ApiController]
public class StaticIconsController : ControllerBase
{
    private readonly IBlobService _blobService;

    public StaticIconsController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<StaticIconViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadStaticIcons()
    {
        try
        {
            var result = await _blobService.DownloadStaticIconsAsync();
            var icons = new List<StaticIconViewModel>();
            foreach (var item in result)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    item.stream.CopyTo(ms);
                    ms.ToArray();

                    icons.Add(new StaticIconViewModel
                    {
                        Content = ms.ToArray(),
                        ContentType = item.contentType,
                        Url = item.url
                    });
                }

            }

            return Ok(icons);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}