using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI countText;
    
    private Colour colour;
    private ItemType itemType;
    private int count;

    public void Setup(Target target)
    {
        colour = target.Colour;
        itemType = target.ItemType;
        count = target.Count;

        countText.text = count.ToString();
        image.texture = ImageLibrary.Instance.GetSprite(colour, itemType).texture;

        LevelManager.Instance.ItemExecuted += OnItemExecuted;
    }

    private void OnItemExecuted(Item item)
    {
        if (item.Colour == colour)
        {
            count--;
            if (count <= 0)
            {
                LevelManager.Instance.ItemExecuted -= OnItemExecuted;
                TargetManager.Instance.DecreaseTarget();
                Destroy(gameObject);
            }
            countText.text = count.ToString();
        }
    }
}
