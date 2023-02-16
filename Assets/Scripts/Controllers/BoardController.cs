using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardController : MonoBehaviour
{
    public const int Rows = 9;
    public const int Cols = 9;
    
    public const int MinimumMatchCount = 2;
    
    public Transform CellsParent;
    public Transform ItemsParent;
    // public Transform ParticlesParent;
    
    [FormerlySerializedAs("CellPrefab")] [SerializeField] private CellController cellControllerPrefab;

    public readonly CellController[,] Cells = new CellController[Cols, Rows];

    private readonly MatchFinder _matchFinder = new ();
    
    public void Prepare()
    {
        CreateCells();
        PrepareCells();
    }
    
    private void CreateCells()
    {
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Cols; x++)
            {
                var cell = Instantiate(cellControllerPrefab, Vector3.zero, Quaternion.identity, CellsParent);
                Cells[x, y] = cell;
            }
        }
    }
    
    private void PrepareCells()
    {
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Cols; x++)
            {
                Cells[x, y].Prepare(x, y, this);
            }
        }
    }
    
    public void CellTapped(CellController cellController)
    {
        if (cellController == null) return;

        if (!cellController.HasItem()) return;

        // if (cell.Item.transform.childCount == 2)
        // {
        //     SpecialistExplode(cell);
        // }
			     //
        // HintManager.isRunning = false;
        // if ( cell.HasItem() && cell.Item._itemType == ItemType.Bomb)
        // {
        //     ExplodeBomb(cell);
        //     return;
        // }
        // else if( cell.HasItem() && (cell.Item._itemType == ItemType.VerticalRocket || cell.Item._itemType == ItemType.HorizontalRocket))
        // {
        //     ExplodeRocket(cell);
        // }
        ExplodeMatchingCells(cellController);
    }
    
    private void ExplodeMatchingCells(CellController cellController)
    {
        var neighboursUnq=new HashSet<CellController>();
        if (cellController == null) return;
        if(cellController.Item == null) return;
        var cells = _matchFinder.FindMatches(cellController, cellController.Item.MatchType);
        if (cells.Count < MinimumMatchCount) return;
        CellController oldCellController = cellController;
        MatchType oldmatchType = oldCellController.Item.MatchType;
        for (var i = 0; i < cells.Count; i++)
        {
            var explodedCell = cells[i];
            var item = explodedCell.Item;
            UniqNeighbour(neighboursUnq,cells[i].Neighbours);
            item.TryExecute();
        }
        ExplodeNeighbourItem(neighboursUnq, oldmatchType);

        // if (cells.Count >= 7) CreateBomb(oldCell);
        // else if(cells.Count < 7 && cells.Count >=5) CreateRocket(oldCell);
    }
    
    private void UniqNeighbour(HashSet<CellController> unqCells,List<CellController> cells)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            unqCells.Add(cells[i]);
        }
    }
    
    private void ExplodeNeighbourItem(HashSet<CellController> cells, MatchType tappedMatchType)
    {
        foreach (var cell in cells)
        {
            if (cell == null) continue;
            if (cell.Item == null) continue;

            // if (cell.Item._itemType == ItemType.Balloon)
            // {
            //     ExplodeColorBalloon(cell, tappedMatchType);
            // }

            else if (cell.Item.ItemType == ItemType.Crate)
            {
                cell.Item.TryExecute();
            }
        }
    }
    
    public CellController GetNeighbourWithDirection(CellController cellController, Direction direction)
    {
        var x = cellController.X;
        var y = cellController.Y;
        switch (direction)
        {
            case Direction.None:
                break;
            case Direction.Up:
                y += 1;
                break;
            case Direction.UpRight:
                y += 1;
                x += 1;
                break;
            case Direction.Right:
                x += 1;
                break;
            case Direction.DownRight:
                y -= 1;
                x += 1;
                break;
            case Direction.Down:
                y -= 1;
                break;
            case Direction.DownLeft:
                y -= 1;
                x -= 1;
                break;
            case Direction.Left:
                x -= 1;
                break;
            case Direction.UpLeft:
                y += 1;
                x -= 1;
                break;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }

        if (x >= Cols || x < 0 || y >= Rows || y < 0) return null;

        return Cells[x, y];
    }
}
