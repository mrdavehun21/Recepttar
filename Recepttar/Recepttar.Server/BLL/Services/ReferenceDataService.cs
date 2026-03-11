using AutoMapper;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;

namespace Recepttar.Server.BLL.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IMapper _mapper;

        public ReferenceDataService(IReferenceDataRepository referenceDataRepository, IMapper mapper)
        {
            _referenceDataRepository = referenceDataRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IngredientSearchDto>> SearchTagsAsync(string? search)
        {
            var ingredients = await _referenceDataRepository.SearchAsync(search);
            return _mapper.Map<IEnumerable<IngredientSearchDto>>(ingredients);
        }

        public IEnumerable<string> GetUnits()
        {
            return Enum.GetValues<MeasurementUnitEnum>().Select(u => u.ToString()).ToList();
        }
    }
}
