using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputSystem : MonoBehaviour
{
    private const string CellCollider = "CellCollider";
    
    [SerializeField] private Camera cam;
    [SerializeField] private BoardController boardController;
    
    void Update ()
    {
#if UNITY_EDITOR
        GetTouchEditor();
#else
			GetTouchMobile();
#endif
    }
    
    private void GetTouchEditor()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ExecuteTouch(Input.mousePosition);
        }
    }

    private void GetTouchMobile()
    {
        var touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                ExecuteTouch(touch.position);
                break;
        }
    }
    
    private void ExecuteTouch(Vector3 pos)
    {
        var hit = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(pos)) as BoxCollider2D;

        if (hit !=null && hit.CompareTag(CellCollider))
        {
            boardController.CellTapped(hit.gameObject.GetComponent<CellController>());
        }
    }
}
