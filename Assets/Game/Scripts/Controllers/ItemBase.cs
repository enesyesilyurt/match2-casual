using Casual.Utilities;
using UnityEngine;

namespace Casual.Controllers
{
    public class ItemBase : MonoBehaviour
    {
        [SerializeField] private FallAnimation fallAnimation;

        public FallAnimation FallAnimation => fallAnimation;

        public void Prepare()
        {
            gameObject.SetActive(true);
            transform.SetParent(BoardController.Instance.ItemsParent);
        }
    }
}
