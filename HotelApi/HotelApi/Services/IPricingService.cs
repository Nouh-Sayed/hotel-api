using HotelApi.DTOs;

namespace HotelApi.Services
{
    public interface IPricingService
    {
        Task<PricingResultDto> CalculateAsync(PricingCalculationDto dto);
        Task<IEnumerable<PricingRuleDto>> GetRulesAsync();
        Task<PricingRuleDto> CreateRuleAsync(CreatePricingRuleDto dto);
        Task DeleteRuleAsync(int id);
    }
}