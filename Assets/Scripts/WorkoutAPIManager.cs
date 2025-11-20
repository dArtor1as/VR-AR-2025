using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


[System.Serializable]
public class WgerResponse
{
    public List<ExerciseInfoResult> results;
}

[System.Serializable]
public class ExerciseInfoResult
{
    public int id;
    public List<ExerciseTranslation> translations;
}

[System.Serializable]
public class ExerciseTranslation
{
    public int language;
    public string name;
    public string description;
    
}

public class WorkoutAPIManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statusText;
    public ForceUpdateLayout layoutUpdater;

    // Використовуємо exerciseinfo, бо він дає детальний опис
    private string realUrl = "https://wger.de/api/v2/exerciseinfo/?language=2&limit=1";

    void Start()
    {
        // GetRandomExercise(); 
    }

    public void GetRandomExercise()
    {
        StartCoroutine(FetchExerciseRoutine());
    }

    IEnumerator FetchExerciseRoutine()
    {
        if (statusText) statusText.text = "Завантаження...";
        if (titleText) titleText.text = "";
        if (descriptionText) descriptionText.text = "";

        // Рандомний офсет
        int randomOffset = Random.Range(0, 200);
        string url = $"{realUrl}&offset={randomOffset}";

        Debug.Log($"Requesting: {url}");

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"API Error: {webRequest.error}");
                if (statusText) statusText.text = "Помилка з'єднання";
                if (descriptionText) descriptionText.text = webRequest.error;
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                ParseAndDisplayData(jsonResult);
            }
        }
    }
    IEnumerator UpdateLayoutNextFrame()
    {
        yield return null; // Чекаємо 1 кадр
        layoutUpdater.UpdateLayout();
    }
    void ParseAndDisplayData(string json)
    {
        try
        {
            WgerResponse response = JsonUtility.FromJson<WgerResponse>(json);

            if (response.results != null && response.results.Count > 0)
            {
                ExerciseInfoResult exercise = response.results[0];
                
                // Шукаємо переклад англійською (ID 2)
                ExerciseTranslation englishTranslation = null;

                if (exercise.translations != null)
                {
                    foreach (var t in exercise.translations)
                    {
                        if (t.language == 2) 
                        {
                            englishTranslation = t;
                            break;
                        }
                    }
                    // Якщо не знайшли англійську, беремо першу ліпшу
                    if (englishTranslation == null && exercise.translations.Count > 0)
                    {
                        englishTranslation = exercise.translations[0];
                    }
                }

                if (englishTranslation != null)
                {
                    string name = string.IsNullOrEmpty(englishTranslation.name) ? "Без назви" : englishTranslation.name;
                    
                    string rawDesc = englishTranslation.description;
                    string cleanDesc = string.IsNullOrEmpty(rawDesc) ? "Опис відсутній" : StripHTML(rawDesc);

                    if (titleText) titleText.text = name;
                    if (descriptionText) descriptionText.text = cleanDesc;
                    if (statusText) statusText.text = ""; 
                }
                else
                {
                    if (statusText) statusText.text = "Переклад не знайдено.";
                    if (titleText) titleText.text = "Вправа #" + exercise.id;
                }
            }
            else
            {
                if (statusText) statusText.text = "Пустий результат (спробуйте ще раз).";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Parse Error: {e.Message}");
            if (statusText) statusText.text = "Помилка обробки JSON.";
        }
        if (layoutUpdater != null) 
        {
            // Чекаємо кадр, щоб текст встиг змінитися, а потім оновлюємо
            StartCoroutine(UpdateLayoutNextFrame());
        }
    }

    string StripHTML(string input)
    {
        if (input == null) return "";
        // Замінюємо <br> та <p> на перенос рядка для кращого вигляду
        string text = input.Replace("<br>", "\n").Replace("<p>", "").Replace("</p>", "\n\n");
        return Regex.Replace(text, "<.*?>", string.Empty).Trim();
    }
}