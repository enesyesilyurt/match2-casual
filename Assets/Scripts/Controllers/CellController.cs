using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CellController : MonoBehaviour
{
    [HideInInspector] public int X;
    [HideInInspector] public int Y;

    [HideInInspector] public CellController firstCellControllerBelow;
    [HideInInspector] public bool IsFillingCell;
    
    public List<CellController> Neighbours { get; private set; }
		
    private Item _item;

    public BoardController BoardController { get; private set; }
    
    public Item Item
    {
        get => _item;
        set
        {
            if (_item == value) return;
				
            var oldItem = _item;
            _item = value;
				
            if (oldItem != null && Equals(oldItem.CellController, this))
            {
                oldItem.CellController = null;
            }
            if (value != null)
            {
                value.CellController = this;
            }
        }
    }
    
    public void Prepare(int x, int y, BoardController boardController)
    {
        X = x;
        Y = y;
        transform.localPosition = new Vector3(x,y);
        IsFillingCell = Y == BoardController.Rows - 1;
        BoardController = boardController;
			
        UpdateLabel();
        UpdateNeighbours(BoardController);
    }
    
    public void ResetNeighbours(BoardController boardController)
    {
        Neighbours.Clear();
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

        if (down != null) firstCellControllerBelow = down;
    }
    
    private void UpdateLabel()
    {
        var cellName = X + ":" + Y;
        gameObject.name = "Cell "+cellName;
    }

    public bool HasItem()
    {
        return Item != null;
    }

    public CellController GetFallTarget()
    {
        var targetCell = this;
        while (targetCell.firstCellControllerBelow != null && targetCell.firstCellControllerBelow.Item == null)
        {
            targetCell = targetCell.firstCellControllerBelow;
        }
        return targetCell;
    }
}
