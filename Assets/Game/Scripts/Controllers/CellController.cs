using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using Unity.Mathematics;
using UnityEngine;

namespace Casual.Controllers
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private Transform mask;
        [SerializeField] private Transform backGround;
        
        private int row;
        private int column;
        private CellController firstCellBelow;
        private bool isFillingCell;
        private Item item;

        public int Row => row;
        public int Column => column;
        public CellController FirstCellBelow => firstCellBelow;
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
                    value.SetSortingOrder(column);
                }
            }
        }
        
        public void Prepare(int row, int column, BoardController boardController)
        {
            this.row = row;
            this.column = column;
            transform.localPosition = new Vector3(row * GameManager.Instance.OffsetX, column * GameManager.Instance.OffsetY);
            GetComponent<BoxCollider2D>().size = new Vector2(GameManager.Instance.OffsetX, GameManager.Instance.OffsetY);

            mask.localScale = new Vector3(GameManager.Instance.OffsetX, GameManager.Instance.OffsetY, 1);
            backGround.localScale = new Vector3(GameManager.Instance.OffsetX, GameManager.Instance.OffsetY, 1);
            
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

            if (up == null)
            {
                var border = SimplePool.Spawn(GameManager.Instance.Border, transform.position, Quaternion.identity);
                border.SetActive(true);
                border.transform.parent = transform;
                isFillingCell = true;
            }
            if (down == null)
            {
                var border = SimplePool.Spawn(GameManager.Instance.Border, transform.position, Quaternion.Euler(0,0,180));
                border.SetActive(true);
                border.transform.parent = transform;
            }
            if (left == null)
            {
                var border = SimplePool.Spawn(GameManager.Instance.Border, transform.position, Quaternion.Euler(0,0,90));
                border.SetActive(true);
                border.transform.parent = transform;
            }
            if (right == null)
            {
                var border = SimplePool.Spawn(GameManager.Instance.Border, transform.position, Quaternion.Euler(0,0,270));
                border.SetActive(true);
                border.transform.parent = transform;
            }
			    
            if(up!=null) Neighbours.Add(up);
            if(down!=null) Neighbours.Add(down);
            if(left!=null) Neighbours.Add(left);
            if(right!=null) Neighbours.Add(right);

            if (down != null) firstCellBelow = down;
        }
        
        private void UpdateLabel()
        {
            var cellName = Row + ":" + Column;
            gameObject.name = "Cell " + cellName;
        }

        public bool HasItem()
        {
            return Item != null;
        }

        public CellController GetFallTarget() // todo
        {
            var targetCell = this;
            while (targetCell.firstCellBelow != null && targetCell.firstCellBelow.Item == null)
            {
                targetCell = targetCell.firstCellBelow;
            }
            return targetCell;
        }
    }
}
