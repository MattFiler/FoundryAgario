using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipResourceManagementUIInteraction : MonoSingleton<ShipResourceManagementUIInteraction>
{
    public void YellowResourcesToEntity()
    {
        ShipResourceManagement.Instance.ConvertResourcesToEntity(ContractAssignee.YELLOW);
    }
    public void RedResourcesToEntity()
    {
        ShipResourceManagement.Instance.ConvertResourcesToEntity(ContractAssignee.RED);
    }
    public void GreenResourcesToEntity()
    {
        ShipResourceManagement.Instance.ConvertResourcesToEntity(ContractAssignee.GREEN);
    }
    public void BlueResourcesToEntity()
    {
        ShipResourceManagement.Instance.ConvertResourcesToEntity(ContractAssignee.BLUE);
    }
}
