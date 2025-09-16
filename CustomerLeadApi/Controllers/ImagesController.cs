using Microsoft.AspNetCore.Mvc;
using CustomerLeadApi.Services;
using CustomerLeadApi.DTOs;

namespace CustomerLeadApi.Controllers
{
    [ApiController]
    [Route("api/customers/{customerId}/images")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerImageDto>>> GetCustomerImages(int customerId)
        {
            try
            {
                var images = await _imageService.GetCustomerImagesAsync(customerId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerImageDto>> UploadImage(int customerId, UploadImageDto uploadImageDto)
        {
            try
            {
                var image = await _imageService.UploadImageAsync(customerId, uploadImageDto);
                if (image == null) return NotFound("Customer not found");
                return CreatedAtAction(nameof(GetCustomerImages), new { customerId }, image);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("batch")]
        public async Task<ActionResult<IEnumerable<CustomerImageDto>>> UploadImages(int customerId, UploadImagesDto uploadImagesDto)
        {
            try
            {
                var images = await _imageService.UploadImagesAsync(customerId, uploadImagesDto);
                return CreatedAtAction(nameof(GetCustomerImages), new { customerId }, images);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int customerId, int imageId)
        {
            try
            {
                var result = await _imageService.DeleteImageAsync(imageId);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetImageCount(int customerId)
        {
            try
            {
                var count = await _imageService.GetImageCountForCustomerAsync(customerId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}