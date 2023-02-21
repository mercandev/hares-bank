using System;
using HB.SharedObject;

namespace HB.Service.Organisation;

public interface IOrganisationService 
{
    Task<ReturnState<object>> GetOrganisations();
}

