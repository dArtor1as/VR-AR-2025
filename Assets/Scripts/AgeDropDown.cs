using UnityEngine;
using TMPro; // Потрібно для Dropdown
using System.Collections.Generic; // Потрібно для List

// Цей скрипт автоматично заповнить Dropdown для віку
[RequireComponent(typeof(TMP_Dropdown))]
public class PopulateAgeDropdown : MonoBehaviour
{
    public int minAge = 18;
    public int maxAge = 25;
    public string firstOptionText = "Оберіть вік...";

    void Start()
    {
        TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
        
        // 1. Очищуємо старі опції
        dropdown.ClearOptions();

        // 2. Створюємо новий список опцій
        List<string> options = new List<string>();
        
        // 3. Додаємо "нульову" опцію
        options.Add(firstOptionText);

        // 4. Додаємо вік від 18 до 25
        for (int i = minAge; i <= maxAge; i++)
        {
            options.Add(i.ToString() + " років");
        }

        // 5. Застосовуємо список до Dropdown
        dropdown.AddOptions(options);
    }
}