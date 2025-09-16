using CustomerLeadApi.DTOs;

namespace CustomerLeadApi.Services
{
    public interface IImageService
    {
        Task<IEnumerable<CustomerImageDto>> GetCustomerImagesAsync(int customerId);
        Task<CustomerImageDto?> UploadImageAsync(int customerId, UploadImageDto uploadImageDto);
        Task<IEnumerable<CustomerImageDto>> UploadImagesAsync(int customerId, UploadImagesDto uploadImagesDto);
        Task<bool> DeleteImageAsync(int imageId);
        Task<int> GetImageCountForCustomerAsync(int customerId);
    }
}