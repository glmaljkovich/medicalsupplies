using ArqNetCore.DTOs.SuppliesOrder;

namespace ArqNetCore.Services
{
    public interface ISuppliesOrderService
    {
        SuppliesOrderCreateResultDTO Create(SuppliesOrderCreateDTO supplyOrderCreateDTO);

    }
}