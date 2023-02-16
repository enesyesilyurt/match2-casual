using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FallAnimation : MonoBehaviour
{
    public Item Item;
    private bool isFalling { get; set; }
    [HideInInspector] public CellController targetCellController;

    private static float _startVel = 0F;
    private static float _acc = 0.3F;
    private static float _maxSpeed = 10F;

    private float _vel = _startVel;

    private Vector3 _targetPosition;
    // private Sequence _jumpSequence;

    public void FallTo(CellController targetCellController)
    {
        if (this.targetCellController != null && targetCellController.Y >= this.targetCellController.Y) return;
        this.targetCellController = targetCellController;
        Item.CellController = this.targetCellController;
        _targetPosition = this.targetCellController.transform.position;
        isFalling = true;
    }

    public void Update()
    {
        if (!isFalling) return;
        _vel += _acc;
        _vel = _vel >= _maxSpeed ? _maxSpeed : _vel;
        var p = Item.transform.position;
        p.y -= _vel * Time.deltaTime;
        if (p.y <= _targetPosition.y)
        {
            isFalling = false;
            targetCellController = null;
            p.y = _targetPosition.y;
            _vel = _startVel;
        }

        Item.transform.position = p;
    }
}
