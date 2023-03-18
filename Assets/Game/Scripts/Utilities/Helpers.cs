using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    static private List<RaycastResult> results = new List<RaycastResult>();
    public static bool IsPointerOverUIObject(Vector2 pos)
    {
        bool isPointerOverUI = false;


        PointerEventData eventDataCurrentPosition =
            new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(
            pos.x,
            pos.y
        );

        results.Clear();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        isPointerOverUI = results.Count > 0;

        return isPointerOverUI;
    }
}
