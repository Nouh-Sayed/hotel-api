using HotelApi.Data;
using HotelApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _context;

        public HotelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<HotelDto>> GetAllAsync(HotelQueryDto queryDto)
        {
            var page = queryDto.Page < 1 ? 1 : queryDto.Page;
            var pageSize = queryDto.PageSize < 1 ? 12 : queryDto.PageSize;

            var query = _context.Hotels
                .Include(h => h.HotelAmenities)
                    .ThenInclude(ha => ha.Amenity)
                .Include(h => h.Images)
                .Include(h => h.PhotoSource)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryDto.Search))
            {
                var search = queryDto.Search.Trim().ToLower();
                query = query.Where(h =>
                    h.Name.ToLower().Contains(search) ||
                    h.City.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(queryDto.City))
            {
                var city = queryDto.City.Trim().ToLower();
                query = query.Where(h => h.City.ToLower() == city);
            }

            if (queryDto.MinPrice.HasValue)
            {
                query = query.Where(h => h.PricePerNightTry >= queryDto.MinPrice.Value);
            }

            if (queryDto.MaxPrice.HasValue)
            {
                query = query.Where(h => h.PricePerNightTry <= queryDto.MaxPrice.Value);
            }

            if (queryDto.Rating.HasValue)
            {
                query = query.Where(h => h.Rating >= queryDto.Rating.Value);
            }

            var totalCount = await query.CountAsync();

            var hotels = await query
                .OrderBy(h => h.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<HotelDto>
            {
                Data = hotels.Select(Map).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<HotelDto?> GetByExternalIdAsync(int externalId)
        {
            var hotel = await _context.Hotels
                .Include(h => h.HotelAmenities)
                    .ThenInclude(ha => ha.Amenity)
                .Include(h => h.Images)
                .Include(h => h.PhotoSource)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(h => h.ExternalId == externalId);

            return hotel == null ? null : Map(hotel);
        }

        private static HotelDto Map(Models.Hotel h)
        {
            return new HotelDto
            {
                HotelId = h.HotelId,
                ExternalId = h.ExternalId,
                Name = h.Name,
                City = h.City,
                Country = h.Country,
                CountryCode = h.CountryCode,
                Address = h.Address,
                Rating = h.Rating,
                Lat = h.Lat,
                Lng = h.Lng,
                PricePerNightTry = h.PricePerNightTry,
                Currency = h.Currency,
                PriceType = h.PriceType,
                Amenities = h.HotelAmenities.Select(x => x.Amenity.Name).ToList(),
                Images = h.Images.Select(x => x.ImageUrl).ToList(),
                GoogleImagesSearch = h.PhotoSource?.GoogleImagesSearch,
                GoogleMapsSearch = h.PhotoSource?.GoogleMapsSearch
            };
        }
    }
}