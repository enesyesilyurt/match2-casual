using System.Collections;
using System.Collections.Generic;
using Casual;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using UnityEngine;

public class PropellerItem : Item, IInitializableWithoutData, IExecutableWithTap, IExecutableWithSpecial, IMovable
{
    public bool CanMove
    {
        get
        {
            return CellController.GetFirstCellBelow() != null &&
                   !CellController.GetFirstCellBelow().HasItem();
        }
    }
    
    public void InitializeWithoutData()
    {
        var propellerSprite = ImageLibrary.Instance.GetSprite(nameof(PropellerItem));
        Prepare(propellerSprite);
    }

    public void Execute()
    {
        var cell = CellController;
        PrepareExecute();
        RemoveItem();
        foreach (var neighbor in cell.GetNeighbours())
        {
            if (neighbor != null && neighbor.HasItem())
            {
                var item = (IExecutableWithSpecial)neighbor.Item;
                item?.ExecuteWithSpecial();
            }
        }
        FallAndFillManager.Instance.Process();
    }

    public void PrepareExecute()
    {
        PrepareRemove();
    }

    public void Move()
    {
        FallAnimation.FallToTarget(CellController.GetFallTarget());
    }
    
    public void ExecuteWithSpecial()
    {
        Execute();
    }

    public void ExecuteWithTap()
    {
        Execute();
    }
}
