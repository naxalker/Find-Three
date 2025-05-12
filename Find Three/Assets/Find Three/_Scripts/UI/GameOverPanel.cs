using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private Button _restartButton;
    private Image _panelImage;

    private const float PANEL_ANIMATION_DURATION = 0.5f;
    private const float CONTENT_ANIMATION_DURATION = 0.4f;
    private const float CONTENT_DELAY = 0.2f;
    private const float PANEL_ALPHA = 0.7f;

    private void Awake()
    {
        _panelImage = GetComponent<Image>();
    }

    public Button RestartButton => _restartButton;

    public void Show(bool isVictory)
    {
        _gameOverText.text = isVictory ? "Victory!" : "Game Over...";

        gameObject.SetActive(true);

        DOTween.Kill(_panelImage);
        DOTween.Kill(_gameOverText);
        DOTween.Kill(_restartButton.transform);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_panelImage.DOFade(PANEL_ALPHA, PANEL_ANIMATION_DURATION)
            .From(0f));

        sequence.Append(_gameOverText.DOFade(1f, CONTENT_ANIMATION_DURATION)
            .From(0f)
            .SetEase(Ease.OutCubic)
            .SetDelay(CONTENT_DELAY));

        sequence.Append(_restartButton.transform.DOScale(Vector3.one, CONTENT_ANIMATION_DURATION)
            .From(Vector3.zero)
            .SetEase(Ease.OutBack));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
