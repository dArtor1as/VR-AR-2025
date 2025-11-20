using UnityEngine;
using OVR;

public class ObjectInfoDisplay : MonoBehaviour
{
    [Header("UI Prefab")]
    public GameObject infoPanelPrefab;

    [Header("Settings")]
    public Transform spawnPoint;
    public OVRInput.Button infoButton = OVRInput.Button.Two; // Кнопка "B" / "N"

    private GameObject currentInfoPanel;
    private bool isHovered = false;

    void Update()
    {
        // Працює ТІЛЬКИ коли промінь наведений на об'єкт
        if (isHovered)
        {
            if (OVRInput.GetDown(infoButton))
            {
                ToggleInfoPanel();
            }
        }
    }

    // Цей метод викликає Event Wrapper
    public void SetHovered(bool hovered)
    {
        isHovered = hovered;
        

    }

    private void ToggleInfoPanel()
    {
        // Якщо панелі немає - створюємо
        if (currentInfoPanel == null)
        {
            if (infoPanelPrefab != null && spawnPoint != null)
            {
                currentInfoPanel = Instantiate(infoPanelPrefab, spawnPoint.position, spawnPoint.rotation);
                
                currentInfoPanel.SetActive(true); 

                // Розвертаємо до гравця
                if (Camera.main != null)
                {
                    currentInfoPanel.transform.LookAt(Camera.main.transform);
                    currentInfoPanel.transform.Rotate(0, 180, 0);
                }
                Debug.Log("Панель відкрито.");
            }
        }
        // Якщо панель вже є - закриваємо
        else
        {
            Destroy(currentInfoPanel);
            currentInfoPanel = null;
            Debug.Log("Панель закрито.");
        }
    }
}