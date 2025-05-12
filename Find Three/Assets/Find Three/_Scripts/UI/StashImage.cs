using UnityEngine;
using UnityEngine.UI;

public class StashImage : MonoBehaviour
{
    [SerializeField] private Image _figureImage;
    [SerializeField] private Image _creatureImage;

    private void Awake()
    {
        _figureImage.gameObject.SetActive(false);
    }

    public void Show(Sprite figureSprire, Color figureColor, Sprite creatureSprite)
    {
        _figureImage.sprite = figureSprire;
        _figureImage.color = figureColor;
        _figureImage.preserveAspect = true;
        _figureImage.type = Image.Type.Simple;

        _creatureImage.sprite = creatureSprite;

        _figureImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _figureImage.gameObject.SetActive(false);
    }
}
