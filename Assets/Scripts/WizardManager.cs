using UnityEngine;
using TMPro; // Потрібно для Text та Dropdown
using UnityEngine.UI; // Потрібно для Button
using System.Collections.Generic; // Потрібно для List<>
using System.Text; // Потрібно для StringBuilder

[System.Serializable]
public class UserData
{
    public string userName;
    public string userSurname; // НОВЕ ПОЛЕ
    public string userAge;
    public string userGender;
}

[System.Serializable]
public class UserList
{
    public List<UserData> users = new List<UserData>();
}


public class WizardManager : MonoBehaviour
{
    [Header("Pages (Сторінки)")]
    public GameObject page0_Welcome;
    public GameObject page1_Name;
    public GameObject page2_Details;
    public GameObject page3_Summary;

    [Header("Main Page UI (Page 0)")]
    public TextMeshProUGUI userListText;

    [Header("Data Fields (Page 1)")]
    public TMP_Dropdown userNameDropdown; 
    public TMP_Dropdown userSurnameDropdown; 
    public Button buttonNextPage1; 

    [Header("Data Fields (Page 2)")]
    public TMP_Dropdown userAgeDropdown; 
    public TMP_Dropdown userGenderDropdown; 
    public Button buttonNextPage2; 

    [Header("Confirmation Text (Page 3)")]
    public TextMeshProUGUI summaryText;

    private const string UserListKey = "Wizard_UserList";
    private UserList allUsers = new UserList();

    void Start()
    {
        LoadData();
        GoToPage(page0_Welcome);
        
        // Ми "слухаємо" подію onValueChanged, щоб перевіряти, чи можна увімкнути кнопку
        if (userNameDropdown != null) userNameDropdown.onValueChanged.AddListener(delegate { CheckPage1Validity(); });
        if (userSurnameDropdown != null) userSurnameDropdown.onValueChanged.AddListener(delegate { CheckPage1Validity(); });
        if (userAgeDropdown != null) userAgeDropdown.onValueChanged.AddListener(delegate { CheckPage2Validity(); });
        if (userGenderDropdown != null) userGenderDropdown.onValueChanged.AddListener(delegate { CheckPage2Validity(); });

        // Встановлюємо початковий стан кнопок (вони будуть вимкнені)
        CheckPage1Validity();
        CheckPage2Validity();
    }

    public void CheckPage1Validity()
    {
        // Кнопка "Далі" активна, тільки якщо в обох Dropdown вибрано щось,
        // крім першого пункту "Виберіть..." (який має індекс 0)
        bool isValid = userNameDropdown.value > 0 && userSurnameDropdown.value > 0;
        if (buttonNextPage1 != null)
        {
            buttonNextPage1.interactable = isValid;
        }
    }

    public void CheckPage2Validity()
    {
        bool isValid = userAgeDropdown.value > 0 && userGenderDropdown.value > 0;
        if (buttonNextPage2 != null)
        {
            buttonNextPage2.interactable = isValid;
        }
    }

    // --- 1. ЛОГІКА ВІЗАРДА (Завдання 4) ---

    public void GoToPage(GameObject pageToShow)
    {
        if (page0_Welcome != null) page0_Welcome.SetActive(false);
        if (page1_Name != null) page1_Name.SetActive(false);
        if (page2_Details != null) page2_Details.SetActive(false);
        if (page3_Summary != null) page3_Summary.SetActive(false);

        if (pageToShow != null)
        {
            pageToShow.SetActive(true);
        }
    }

    public void ShowSummary()
    {
        if (summaryText != null)
        {
            string nameString = userNameDropdown.options[userNameDropdown.value].text;
            string surnameString = userSurnameDropdown.options[userSurnameDropdown.value].text;
            string ageString = userAgeDropdown.options[userAgeDropdown.value].text;
            string genderString = userGenderDropdown.options[userGenderDropdown.value].text;

            summaryText.text = $"Ім'я: {nameString}\n" +
                               $"Прізвище: {surnameString}\n" +
                               $"Вік: {ageString}\n" +
                               $"Стать: {genderString}";
        }
        GoToPage(page3_Summary);
    }

    // --- 2. ЗБЕРЕЖЕННЯ/ЗАВАНТАЖЕННЯ (Завдання 3) ---

    public void RegisterAndFinish()
    {
        UserData newUser = new UserData
        {
            userName = userNameDropdown.options[userNameDropdown.value].text,
            userSurname = userSurnameDropdown.options[userSurnameDropdown.value].text,
            userAge = userAgeDropdown.options[userAgeDropdown.value].text,
            userGender = userGenderDropdown.options[userGenderDropdown.value].text
        };

        allUsers.users.Add(newUser);
        SaveData();
        RefreshUserListText();
        GoToPage(page0_Welcome);
        ClearSelections();
    }
    
    private void SaveData()
    {
        string json = JsonUtility.ToJson(allUsers);
        PlayerPrefs.SetString(UserListKey, json);
        PlayerPrefs.Save();
        Debug.Log("Список користувачів збережено в JSON!");
    }

    private void LoadData()
    {
        string json = PlayerPrefs.GetString(UserListKey, "{}");
        allUsers = JsonUtility.FromJson<UserList>(json);
        
        if (allUsers == null || allUsers.users == null)
        {
            allUsers = new UserList();
            allUsers.users = new List<UserData>();
        }

        RefreshUserListText();
    }

    private void RefreshUserListText()
    {
        if (userListText == null) return;

        if (allUsers.users.Count == 0)
        {
            userListText.text = "Ще ніхто не зареєструвався.";
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (var user in allUsers.users)
        {
            sb.AppendLine($"- {user.userName} {user.userSurname} ({user.userAge}), {user.userGender}");
        }

        userListText.text = sb.ToString();
    }
    
    private void ClearSelections()
    {
        userNameDropdown.value = 0;
        userSurnameDropdown.value = 0;
        userAgeDropdown.value = 0;
        userGenderDropdown.value = 0;
    }
    
    [ContextMenu("!!! ОЧИСТИТИ ВСІ ДАНІ !!!")]
    public void ClearAllData()
    {
        PlayerPrefs.DeleteKey(UserListKey);
        PlayerPrefs.Save();
        allUsers = new UserList();
        allUsers.users = new List<UserData>();
        RefreshUserListText();
        Debug.LogWarning("УСІ ЗБЕРЕЖЕНІ ДАНІ КОРИСТУВАЧІВ ВИДАЛЕНО.");
    }
}