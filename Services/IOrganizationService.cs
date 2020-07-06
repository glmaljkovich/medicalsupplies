using ArqNetCore.DTOs.Organization;

namespace ArqNetCore.Services
{
    public interface IOrganizationService
    {
        
        OrganizationListResultDTO List();
        OrganizationGroupBySupplyTypeResultDTO GroupBySupplyType();

        

    }
}