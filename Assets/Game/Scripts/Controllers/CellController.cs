using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using UnityEngine;

namespace Casual.Controllers
{
    public class CellController : MonoBehaviour
    {
        private int x;
        private int y;
        private CellController _firstCellBelow;
        private bool isFillingCell;
        private Item item;

        public int X => x;
        public int Y => y;
        public CellController FirstCellBelow => _firstCellBelow;
        public bool IsFillingCell => isFillingCell;
        
        public List<CellController> Neighbours { get; private set; }
        
        public Item Item
        {
            get => item;
            set
            {
                if (item == value) return;
				    
                var oldItem = item;
                item = value;
				    
                if (oldItem != null && Equals(oldItem.CellController, this))
                {
                    oldItem.CellController = null;
                }
                if (value != null)
                {
                    value.CellController = this;
                    value.SetSortingOrder(y);
                }
            }
        }
        
        public void Prepare(int x, int y, BoardController boardController)
        {
            this.x = x;
            this.y = y;
            transform.localPosition = new Vector3(x * GameManager.Instance.OffsetX, y * GameManager.Instance.OffsetY);
            GetComponent<BoxCollider2D>().size = new Vector2(GameManager.Instance.OffsetX, GameManager.Instance.OffsetY);
            isFillingCell = Y == LevelManager.Instance.CurrentLevel.Size.y - 1;
            
            UpdateLabel();
            UpdateNeighbours(boardController);
        }
        
        private void UpdateNeighbours(BoardController boardController)
        {
            Neighbours = new List<CellController>();
            var up = boardController.GetNeighbourWithDirection(this, Direction.Up);
            var down = boardController.GetNeighbourWithDirection(this, Direction.Down);
            var left = boardController.GetNeighbourWithDirection(this, Direction.Left);
            var right = boardController.GetNeighbourWithDirection(this, Direction.Right);
			    
            if(up!=null) Neighbours.Add(up);
            if(down!=null) Neighbours.Add(down);
            if(left!=null) Neighbours.Add(left);
            if(right!=null) Neighbours.Add(right);

            if (down != null) _firstCellBelow = down;
        }
        
        private void UpdateLabel()
        {
            var cellName = X + ":" + Y;
            gameObject.name = "Cell " + cellName;
        }

        public bool HasItem()
        {
            return Item != null;
        }

        public CellController GetFallTarget() // todo
        {
            var targetCell = this;
            while (targetCell._firstCellBelow != null && targetCell._firstCellBelow.Item == null)
            {
                targetCell = targetCell._firstCellBelow;
            }
            return targetCell;
        }
    }
}
