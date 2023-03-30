using Casual;
using Casual.Abstracts;
using Casual.Controllers.Items;
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
    private int count;
    private string type;

    public void Setup(Target target)
    {
        colour = target.Colour;
        count = target.Count;
        type = target.ItemType;

        countText.text = count.ToString();
        image.texture = ImageLibrary.Instance.GetSprite(target.ItemType, colour).texture;

        LevelManager.Instance.ItemExecuted += OnItemExecuted;
        GameManager.Instance.GameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        LevelManager.Instance.ItemExecuted -= OnItemExecuted;
        GameManager.Instance.GameStateChanged -= OnGameStateChanged;
        Destroy(gameObject);
    }

    private void OnItemExecuted(Item item)
    {
        var targetableColour = item as ITargetable<Colour>;
        var targetableType = item as ITargetable<string>;
        
        if (targetableColour?.Value == colour || targetableType?.Value == type)
        {
            count--;
            if (count <= 0)
            {
                LevelManager.Instance.ItemExecuted -= OnItemExecuted;
                GameManager.Instance.GameStateChanged -= OnGameStateChanged;
                TargetManager.Instance.DecreaseTarget();
                Destroy(gameObject);
            }
            countText.text = count.ToString();
        }
    }
}
