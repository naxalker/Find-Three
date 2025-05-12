using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager : IInitializable, IDisposable
{
    private const float INTERACTION_DISTANCE = 100f;

    private PlayerInputActions _playerInputActions;

    private LayerMask _interactableLayer = LayerMask.GetMask("Interactable");

    private readonly GameManager _gameManager;

    public InputManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Initialize()
    {
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Gameplay.Enable();
        _playerInputActions.Gameplay.Interact.performed += OnInteractPerformed;
    }

    public void Dispose()
    {
        _playerInputActions.Gameplay.Interact.performed -= OnInteractPerformed;
        _playerInputActions.Gameplay.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (_gameManager.IsInputActive == false) { return; }

        Vector2 screenPosition;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            screenPosition = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.isInProgress)
        {
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else
        {
            throw new InvalidOperationException("No valid input source detected.");
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, INTERACTION_DISTANCE, _interactableLayer.value == 0 ? ~0 : _interactableLayer);

        if (hit2D.collider != null)
        {
            Debug.Log("Hit 2D: " + hit2D.collider.gameObject.name);

            if (hit2D.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
}
