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
        [SerializeField] private GameObject[] borders;

        private Vector2Int gridPosition;
        private bool isFillingCell;
        private Item item;
        private Obstacle obstacle;

        public Vector2Int GridPosition => gridPosition;
        public bool IsFillingCell => isFillingCell;
        
        public bool IsItemCanTap { get; set; } = true;

        public bool IsItemCanFall { get; set; } = true;
        public bool IsItemCanExecute { get; set; } = true;

        public Item Item
        {
            get => item;
            set
            {
                if (item == value) return;
                
                item = value;
                
                if (value != null)
                {
                    value.CellController = this;
                    value.SetSortingOrder(gridPosition.y);
                }
            }
        }
        
        public Obstacle Obstacle
        {
            get => obstacle;
            set
            {
                if (obstacle == value) return;
                
                obstacle = value;
                
                if (value != null)
                {
                    value.CellController = this;
                    value.SetSortingOrder(gridPosition.y + 1);
                }
            }
        }

        public void Initialize(int row, int column, Transform parent)
        {
            gridPosition = new Vector2Int(row, column);
            transform.localPosition = new Vector3(row * GameManager.Instance.OffsetX, column * GameManager.Instance.OffsetY);
            gameObject.SetActive(true);
            transform.SetParent(parent);
        }

        public bool CanTap()
        {
            return IsItemCanTap && HasItem() && !item.FallAnimation.IsFalling;
        }
        
        public void Prepare()
        {
            GetComponent<BoxCollider2D>().size = new Vector2(GameManager.Instance.OffsetX, GameManager.Instance.OffsetY);

            mask.localScale = new Vector3(GameManager.Instance.OffsetX, GameManager.Instance.OffsetY, 1);
            backGround.localScale = new Vector3(GameManager.Instance.OffsetX, GameManager.Instance.OffsetY, 1);
            if (BoardController.Instance.GetCell(GridPosition + Vector2Int.up) == null)
                isFillingCell = true;
            
            UpdateLabel();
            CreateBorder();
        }

        public void ItemExecuted()
        {
            if (obstacle != null)
            {
                obstacle.OnItemExecuted();
            }
        }

        private void CreateBorder()
        {
            for (int i = 0; i < 4; i++)
            {
                borders[i].SetActive(false);
                if (BoardController.Instance.GetCell(gridPosition + BoardController.directions[i]) == null)
                {
                    borders[i].SetActive(true);
                    borders[i].transform.rotation = Quaternion.Euler(0, 0, i * -90);
                }
            }
        }
        
        private void UpdateLabel()
        {
            var cellName = gridPosition.x + ":" + gridPosition.y;
            gameObject.name = "Cell " + cellName;
        }

        public bool HasItem()
        {
            return Item != null;
        }
        
        public bool HasObstacle()
        {
            return Obstacle != null;
        }

        public CellController GetFallTarget() // todo
        {
            var targetCell = this;
            
            while (targetCell.GetFirstCellBelow() != null && !targetCell.GetFirstCellBelow().HasItem())
            {
                targetCell = targetCell.GetFirstCellBelow();
            }
            return targetCell;
        }

        public CellController[] GetNeighbours()
        {
            var neighbors = new CellController[4];
            for (int i = 0; i < 4; i++)
            {
                neighbors[i] = BoardController.Instance.GetCell(GridPosition + BoardController.directions[i]);
            }

            return neighbors;
        }

        public CellController GetFirstCellBelow()
        {
            return BoardController.Instance.GetCell(GridPosition + Vector2Int.down);
        }
    }
}
