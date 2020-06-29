using ArqNetCore.DTOs.SuppliesOrder;

namespace ArqNetCore.Services
{
    public interface ISuppliesOrderService
    {
        SuppliesOrderCreateResultDTO Create(SuppliesOrderCreateDTO supplyOrderCreateDTO);

        SuppliesOrderListResultDTO List();

        SuppliesOrderSupplyTypesResultDTO SupplyTypes();

        SuppliesOrderGetByIdResultDTO GetById(int id);

        SuppliesOrderAcceptResultDTO Accept(SuppliesOrderAcceptDTO suppliesOrderAcceptDTO);

        SuppliesOrderRejectResultDTO Reject(SuppliesOrderRejectDTO suppliesOrderRejectDTO);

    }
}