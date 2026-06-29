using UnityEngine;
// Yeni input sistemini kullanabilmek için namespace ekliyoruz
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Eski sistem: Input.GetMouseButtonDown(0) yerine:
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }

        // Ýleride mobil destek eklemek istersen tek satýrla dokunmayý da ekleyebilirsin:
        // if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame) { ... }
    }

    private void HandleClick()
    {
        // Eski sistem: Input.mousePosition yerine Mouse.current.position okuyoruz
        Vector2 screenPosition = Mouse.current.position.ReadValue();
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