using UnityEngine;
using OVR; 

public class ToggleMenu : MonoBehaviour
{

    public GameObject menuCanvas;
    
    public Transform cameraTarget; 

    [Header("Spawn Settings")]
    public float spawnDistance = 1.5f; // Як далеко попереду з'явиться
    public float spawnHeightOffset = -0.3f; // Наскільки нижче рівня очей

    void Update()
    {
        if (cameraTarget == null || menuCanvas == null) return;

        if (OVRInput.GetDown(OVRInput.Button.Start))
        {

            bool isMenuActive = menuCanvas.activeSelf;

            menuCanvas.SetActive(!isMenuActive);

            if (!isMenuActive)
            {
                PositionMenu();
            }
        }
    }

    // Ця функція розміщує меню перед гравцем
    void PositionMenu()
    {
        // Беремо напрямок погляду камери (тільки по горизонталі)
        Vector3 lookDirection = cameraTarget.forward;
        lookDirection.y = 0; 
        lookDirection.Normalize();

        // Розраховуємо позицію: 
        // позиція камери + 1.5м вперед + 0.3м вниз
        Vector3 targetPosition = cameraTarget.position 
                               + (lookDirection * spawnDistance) 
                               + (Vector3.up * spawnHeightOffset);
        
        menuCanvas.transform.position = targetPosition;

        // --- 2. Встановлюємо ОБЕРТАННЯ меню ---
        // повернуте до гравця
        Quaternion targetRotation = Quaternion.Euler(0, cameraTarget.eulerAngles.y, 0);
        
        menuCanvas.transform.rotation = targetRotation;
    }
}