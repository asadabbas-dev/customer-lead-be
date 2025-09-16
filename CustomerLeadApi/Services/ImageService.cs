using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CustomerLeadApi.Data;
using CustomerLeadApi.Models;
using CustomerLeadApi.DTOs;

namespace CustomerLeadApi.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private const int MaxImagesPerCustomer = 10;

        public ImageService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerImageDto>> GetCustomerImagesAsync(int customerId)
        {
            var images = await _context.CustomerImages
                .Where(img => img.CustomerId == customerId)
                .OrderByDescending(img => img.UploadedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CustomerImageDto>>(images);
        }

        public async Task<CustomerImageDto?> UploadImageAsync(int customerId, UploadImageDto uploadImageDto)
        {
            // Check if customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!customerExists) return null;

            // Check current image count
            var currentImageCount = await GetImageCountForCustomerAsync(customerId);
            if (currentImageCount >= MaxImagesPerCustomer)
            {
                throw new InvalidOperationException($"Cannot upload more than {MaxImagesPerCustomer} images per customer.");
            }

            // Validate Base64 string
            if (!IsValidBase64(uploadImageDto.ImageData))
            {
                throw new ArgumentException("Invalid Base64 image data.");
            }

            var customerImage = new CustomerImage
            {
                CustomerId = customerId,
                ImageData = uploadImageDto.ImageData,
                FileName = uploadImageDto.FileName,
                ContentType = uploadImageDto.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            _context.CustomerImages.Add(customerImage);
            await _context.SaveChangesAsync();

            return _mapper.Map<CustomerImageDto>(customerImage);
        }

        public async Task<IEnumerable<CustomerImageDto>> UploadImagesAsync(int customerId, UploadImagesDto uploadImagesDto)
        {
            // Check if customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!customerExists)
            {
                throw new ArgumentException("Customer not found.");
            }

            // Check current image count
            var currentImageCount = await GetImageCountForCustomerAsync(customerId);
            var newImagesCount = uploadImagesDto.Images.Count;

            if (currentImageCount + newImagesCount > MaxImagesPerCustomer)
            {
                throw new InvalidOperationException(
                    $"Cannot upload {newImagesCount} images. Customer already has {currentImageCount} images. " +
                    $"Maximum allowed is {MaxImagesPerCustomer} images per customer.");
            }

            var customerImages = new List<CustomerImage>();

            foreach (var uploadImageDto in uploadImagesDto.Images)
            {
                // Validate Base64 string
                if (!IsValidBase64(uploadImageDto.ImageData))
                {
                    throw new ArgumentException($"Invalid Base64 image data for file: {uploadImageDto.FileName}");
                }

                var customerImage = new CustomerImage
                {
                    CustomerId = customerId,
                    ImageData = uploadImageDto.ImageData,
                    FileName = uploadImageDto.FileName,
                    ContentType = uploadImageDto.ContentType,
                    UploadedAt = DateTime.UtcNow
                };

                customerImages.Add(customerImage);
            }

            _context.CustomerImages.AddRange(customerImages);
            await _context.SaveChangesAsync();

            return _mapper.Map<IEnumerable<CustomerImageDto>>(customerImages);
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var image = await _context.CustomerImages.FindAsync(imageId);
            if (image == null) return false;

            _context.CustomerImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetImageCountForCustomerAsync(int customerId)
        {
            return await _context.CustomerImages.CountAsync(img => img.CustomerId == customerId);
        }

        private static bool IsValidBase64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String)) return false;

            try
            {
                // Remove data URL prefix if present (e.g., "data:image/jpeg;base64,")
                var base64Data = base64String.Contains(',') ? base64String.Split(',')[1] : base64String;
                Convert.FromBase64String(base64Data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}