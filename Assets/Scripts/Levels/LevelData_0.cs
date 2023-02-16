using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData_0 : LevelData
{
    public override ItemType GetNextFillItemType()
    {
        return GetRandomCubeItemType();
    }

    public override void Initialize()
    {
        GridData = new ItemType[BoardController.Rows, BoardController.Cols];

        for (var y = 0; y < BoardController.Rows; y++)
        {
            for (var x = 0; x < BoardController.Cols; x++)
            {
                GridData[x, y] = GetRandomCubeItemType();
            }
        }
    }
}
