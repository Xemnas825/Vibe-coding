using Microsoft.AspNetCore.Mvc;
using VibeApi.Models.External;
using VibeApi.Services.External;

namespace VibeApi.Controllers.External;

[ApiController]
[Route("api/external/[controller]")]
public class PhotosController : ControllerBase
{
    private readonly IExternalPhotoService _photoService;

    public PhotosController(IExternalPhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExternalPhoto>>> GetPhotos()
    {
        var photos = await _photoService.GetPhotosAsync();
        return Ok(photos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExternalPhoto>> GetPhoto(int id)
    {
        var photo = await _photoService.GetPhotoByIdAsync(id);
        return photo == null ? NotFound() : Ok(photo);
    }

    [HttpPost]
    public async Task<ActionResult<ExternalPhoto>> CreatePhoto(ExternalPhoto photo)
    {
        var createdPhoto = await _photoService.CreatePhotoAsync(photo);
        return CreatedAtAction(nameof(GetPhoto), new { id = createdPhoto.Id }, createdPhoto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExternalPhoto>> UpdatePhoto(int id, ExternalPhoto photo)
    {
        var updatedPhoto = await _photoService.UpdatePhotoAsync(id, photo);
        return updatedPhoto == null ? NotFound() : Ok(updatedPhoto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        var success = await _photoService.DeletePhotoAsync(id);
        return success ? NoContent() : NotFound();
    }
}