using System.Collections.Generic;
using Casual.Controllers;
using Casual.Enums;
using Casual.Managers;

namespace Casual.Utilities
{
    public class MatchFinder
    {
        private bool[] visitedCells;

        public void Setup()
        {
            visitedCells = new bool[LevelManager.Instance.CurrentLevel.GridWidth * LevelManager.Instance.CurrentLevel.GridHeight];
        }
        
        public List<CellController> FindMatches(CellController cellController, Colour colour)
        {
            var resultCells = new List<CellController>();
            ClearVisitedCells();
            FindMatches(cellController, colour, resultCells);

            return resultCells;
        }

        private void FindMatches(CellController cellController, Colour colour, List<CellController> resultCells)
        {
            if (cellController == null) return;
			    
            var row = cellController.GridPosition.x;
            var column = cellController.GridPosition.y;
            if (visitedCells[LevelManager.Instance.CurrentLevel.GridWidth * column + row]) return;

            if (cellController.HasItem()
                && cellController.Item.Colour == colour
                && cellController.Item.Colour != Colour.None)
            {
                visitedCells[LevelManager.Instance.CurrentLevel.GridWidth * column + row] = true;
                resultCells.Add(cellController);
			    
                var neighbours = cellController.GetNeighbours();
                if (neighbours.Length == 0) return;
	    
                for (var i = 0; i < neighbours.Length; i++)
                {	
                    FindMatches(neighbours[i], colour, resultCells);
                }
            }
        }

        private void ClearVisitedCells()
        {
            for (var i = 0; i < visitedCells.GetLength(0); i++)
            {
                visitedCells[i] = false;
            }
        }
    }
}
