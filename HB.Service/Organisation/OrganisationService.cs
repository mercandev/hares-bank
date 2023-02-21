using System;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.Repository;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;

namespace HB.Service.Organisation;

public class OrganisationService : IOrganisationService
{
    private readonly IRepository<Organisations> _organisationRepository;
    private readonly IMapper _mapper;


    public OrganisationService(IRepository<Organisations> organisationRepository , IMapper mapper)
    {
        this._organisationRepository = organisationRepository;
        this._mapper = mapper;
    }

    public async Task<ReturnState<object>> GetOrganisations()
    {
        var result = await _organisationRepository.FindToListAsync();

        var mapperResult = _mapper.Map<List<OrganisationsViewModel>>(result);

        return new ReturnState<object>(mapperResult);
    }
}

