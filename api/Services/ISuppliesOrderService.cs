using ArqNetCore.DTOs.SuppliesOrder;

namespace ArqNetCore.Services
{
    public interface ISuppliesOrderService
    {
        SuppliesOrderCreateResultDTO Create(SuppliesOrderCreateDTO supplyOrderCreateDTO);

        SuppliesOrderListResultDTO List();

        SuppliesOrderAcceptResultDTO Accept(SuppliesOrderAcceptDTO suppliesOrderAcceptDTO);

        SuppliesOrderRejectResultDTO Reject(SuppliesOrderRejectDTO suppliesOrderRejectDTO);

    }
}