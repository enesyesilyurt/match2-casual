using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private readonly bool[,] _visitedCells = new bool[BoardController.Rows, BoardController.Cols];
    
    public List<CellController> FindMatches(CellController cellController, MatchType matchType)
    {
        var resultCells = new List<CellController>();
        ClearVisitedCells();
        FindMatches(cellController, matchType, resultCells);

        return resultCells;
    }
    
    public void FindMatches(CellController cellController, MatchType matchType, List<CellController> resultCells)
    {
        if (cellController == null) return;
			
        var x = cellController.X;
        var y = cellController.Y;
        if (_visitedCells[x, y]) return;

        if (cellController.HasItem()
            && cellController.Item.MatchType == matchType
            && cellController.Item.MatchType != MatchType.None && cellController.Item.ItemType != ItemType.Balloon)
        {
            _visitedCells[x, y] = true;
            resultCells.Add(cellController);
			
            var neighbours = cellController.Neighbours;
            if (neighbours.Count == 0) return;
	
            for (var i = 0; i < neighbours.Count; i++)
            {	
                FindMatches(neighbours[i], matchType, resultCells);
            }
        }
    }
    
    public void ClearVisitedCells()
    {
        for (var x = 0; x < _visitedCells.GetLength(0); x++)
        {
            for (var y = 0; y < _visitedCells.GetLength(1); y++)
            {
                _visitedCells[x, y] = false;
            }
        }
    }
}
