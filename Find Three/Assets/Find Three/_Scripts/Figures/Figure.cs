using UnityEngine;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Figure : MonoBehaviour, IInteractable
{
    public static event Action<Figure> OnInteracted;

    private const float TARGET_SCALE = 0.2f;

    [SerializeField] private SpriteRenderer _creatureIcon;
    private SpriteRenderer _bgSpriteRenderer;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;

    private FigureDefinition _figureDefinition;

    public FigureDefinition FigureDefinition => _figureDefinition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _bgSpriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        gameObject.SetActive(false);
    }

    public void Setup(FigureDefinition figureDefinition, Color color, Sprite creatureSprite)
    {
        _figureDefinition = figureDefinition;

        _bgSpriteRenderer.color = color;
        _creatureIcon.sprite = creatureSprite;
    }

    public void Interact()
    {
        Deactivate().Forget();

        OnInteracted?.Invoke(this);
    }

    public async UniTask Activate()
    {
        transform.DOKill();

        gameObject.SetActive(true);
        _collider.enabled = false;
        _rigidbody.simulated = true;

        await transform.DOScale(Vector3.one * TARGET_SCALE, .5f)
            .From(Vector3.zero)
            .SetEase(Ease.OutBack)
            .AsyncWaitForCompletion();

        _collider.enabled = true;
    }

    public async UniTask Deactivate()
    {
        transform.DOKill();

        _collider.enabled = false;
        _rigidbody.simulated = false;

        await transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .AsyncWaitForCompletion();

        Destroy(gameObject);
    }
}
