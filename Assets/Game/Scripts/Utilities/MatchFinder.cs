using System.Collections.Generic;
using Casual.Controllers;
using Casual.Enums;
using Casual.Managers;

namespace Casual.Utilities
{
    public class MatchFinder
    {
        private bool[,] visitedCells;

        public void Setup()
        {
            visitedCells = new bool[LevelManager.Instance.CurrentLevel.Size.x, LevelManager.Instance.CurrentLevel.Size.y];
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
			    
            var x = cellController.X;
            var y = cellController.Y;
            if (visitedCells[x, y]) return;

            if (cellController.HasItem()
                && cellController.Item.Colour == colour
                && cellController.Item.Colour != Colour.None)
            {
                visitedCells[x, y] = true;
                resultCells.Add(cellController);
			    
                var neighbours = cellController.Neighbours;
                if (neighbours.Count == 0) return;
	    
                for (var i = 0; i < neighbours.Count; i++)
                {	
                    FindMatches(neighbours[i], colour, resultCells);
                }
            }
        }
        
        public void ClearVisitedCells()
        {
            for (var x = 0; x < visitedCells.GetLength(0); x++)
            {
                for (var y = 0; y < visitedCells.GetLength(1); y++)
                {
                    visitedCells[x, y] = false;
                }
            }
        }
    }
}
