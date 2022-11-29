using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuPanel, profilePanel;

    [SerializeField]
    Button playBtn, profileBtn, saveBtn, backBtn,loadBtn;

    [SerializeField]
    TMP_InputField profileNameInput;

    [SerializeField]
    TextMeshProUGUI messageText;

    [SerializeField]
    TMP_Dropdown profileSelectDropdown;

    [SerializeField]
    GameDataSO gameData;

    private void OnEnable()
    {
        playBtn.onClick.AddListener(LoadGame);
        profileBtn.onClick.AddListener(ProfileBtnClicked);
        saveBtn.onClick.AddListener(SaveProfile);
        backBtn.onClick.AddListener(BackBtnClicked);
        loadBtn.onClick.AddListener(LoadProfile);
        if (GameDataSO.instance != null)
            return;
        gameData.InitGameData();
        gameData.LoadData();
        gameData.SetCurrentProfile();
    }

    private void OnDisable()
    {
        playBtn.onClick.RemoveListener(LoadGame);
        profileBtn.onClick.RemoveListener(ProfileBtnClicked);
        saveBtn.onClick.RemoveListener(SaveProfile);
        backBtn.onClick.RemoveListener(BackBtnClicked);
        loadBtn.onClick.RemoveListener(LoadProfile);
    }


    void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

     void ProfileBtnClicked()
    {
        menuPanel.SetActive(false);
        profilePanel.SetActive(true);
        messageText.text = "";
        UpdateProfileSelectDropdown();
    }

    private void UpdateProfileSelectDropdown()
    {
        List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < GameDataSO.instance.gameData.profileData.Count; i++)
            optionData.Add(new TMP_Dropdown.OptionData(GameDataSO.instance.gameData.profileData[i].profileName));

        profileSelectDropdown.options = optionData;
    }

    void SaveProfile()
    {
        string profileName = profileNameInput.text;
        if (string.IsNullOrWhiteSpace(profileName))
        {
            messageText.text = "Profile name cannot be blank!";
            return;
        }
            
        ProfileData pd=GameDataSO.instance.gameData.profileData.Find(x => x.profileName == profileName);
        if(pd!=null)
        {
            messageText.text = "Profile already exists!";
            return;
        }
        ProfileData data = new ProfileData();
        data.profileName = profileName;
        GameDataSO.instance.gameData.profileData.Add(data);
        GameDataSO.instance.gameData.currentActiveProfile = data;
        GameDataSO.instance.SaveData();
        profileNameInput.text = "";
        messageText.text = "Profile created successfully!";
        UpdateProfileSelectDropdown();
    }

    void BackBtnClicked()
    {
        profilePanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            profileNameInput.text = "James";
            SaveProfile();
        }
    }

    void LoadProfile()
    {
        GameDataSO.instance.gameData.currentActiveProfile = GameDataSO.instance.gameData.profileData[profileSelectDropdown.value];
        messageText.text = "Profile loaded successfully";
    }


}
