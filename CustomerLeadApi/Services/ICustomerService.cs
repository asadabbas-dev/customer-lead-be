using CustomerLeadApi.DTOs;

namespace CustomerLeadApi.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerDto?> UpdateCustomerAsync(int id, CreateCustomerDto updateCustomerDto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<IEnumerable<CustomerDto>> GetLeadsAsync();
        Task<IEnumerable<CustomerDto>> GetCustomersAsync();
    }
}