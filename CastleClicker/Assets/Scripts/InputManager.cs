using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionsAsset;
    
    private Camera mainCamera;
    private InputAction attackAction;
    private bool isClickQueued;

    private void Awake()
    {
        if (inputActionsAsset != null)
        {
            // Find "Player/Attack" action
            var playerMap = inputActionsAsset.FindActionMap("Player");
            if (playerMap != null)
            {
                attackAction = playerMap.FindAction("Attack");
            }
        }
    }

    private void OnEnable()
    {
        if (attackAction != null)
        {
            attackAction.Enable();
            attackAction.performed += OnAttackPerformed;
        }
    }

    private void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.Disable();
            attackAction.performed -= OnAttackPerformed;
        }
        isClickQueued = false;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 1. Process queued click from event callback safely in Update loop
        if (isClickQueued)
        {
            isClickQueued = false;
            HandleClick();
        }

        // 2. Fallback: If InputActionAsset is not assigned, fall back to polling to prevent breaking
        if (attackAction == null)
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleClick();
            }
            else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                HandleClick();
            }
        }
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        isClickQueued = true;
    }

    private void HandleClick()
    {
        // Prevent click if mouse/pointer is over a UI element (Safe to call in Update loop)
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector2 screenPosition = Vector2.zero;
        if (Pointer.current != null)
        {
            screenPosition = Pointer.current.position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            screenPosition = Mouse.current.position.ReadValue();
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null)
            {
                clickable.OnClick();
            }
        }
    }
}
