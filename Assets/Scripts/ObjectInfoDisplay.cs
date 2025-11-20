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
        // ДІАГНОСТИКА: Якщо наведено, перевіряємо натискання
        if (isHovered)
        {
            if (OVRInput.GetDown(infoButton))
            {
                Debug.Log(">>> КНОПКУ НАТИСНУТО! Перемикаю панель.");
                ToggleInfoPanel();
            }
        }
    }

    public void SetHovered(bool hovered)
    {
        isHovered = hovered;
        
        // ДІАГНОСТИКА: Пишемо в консоль, коли промінь заходить/виходить
        if (hovered) Debug.Log($"XXX НАВЕДЕНО на {gameObject.name}!");
        else Debug.Log($"... пішов з {gameObject.name}.");

        if (!isHovered && currentInfoPanel != null)
        {
            Destroy(currentInfoPanel);
            currentInfoPanel = null;
        }
    }

    private void ToggleInfoPanel()
    {
        if (currentInfoPanel == null)
        {
            if (infoPanelPrefab != null && spawnPoint != null)
            {
                currentInfoPanel = Instantiate(infoPanelPrefab, spawnPoint.position, spawnPoint.rotation);
                currentInfoPanel.SetActive(true);
                Transform cameraTransform = Camera.main.transform; 
            
            Vector3 directionToCamera = currentInfoPanel.transform.position - cameraTransform.position;
            
            directionToCamera.y = 0; 

            currentInfoPanel.transform.rotation = Quaternion.LookRotation(directionToCamera);
                Debug.Log("Панель створено.");
            }
            else
            {
                Debug.LogError("ПОМИЛКА: Не вказано infoPanelPrefab або spawnPoint!");
            }
        }
        else
        {
            Destroy(currentInfoPanel);
            currentInfoPanel = null;
            Debug.Log("Панель прибрано.");
        }
    }
}