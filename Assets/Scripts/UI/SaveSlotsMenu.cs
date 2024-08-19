using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private Mainmenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;

    [Header("StartScene")]
    [SerializeField] private string startSceneName;

    [Header("Save Slot Setup")]
    [SerializeField] private GameObject saveSlotPrefab; // 동적으로 생성할 SaveSlot Prefab
    [SerializeField] private Transform saveSlotContainer; // SaveSlot이 배치될 부모 객체
    [SerializeField] private OpenCloseWindow savePanel;

    [SerializeField] private List<SaveSlot> saveSlots = new List<SaveSlot>();

    private bool isLoadingGame = false;

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // disable all buttons
        DisableMenuButtons();

        // case - loading game
        if (isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameAndLoadScene();
        }
        // case - new game, but the save slot has data
        else
        {
            confirmationPopupMenu.ActivateMenu(
                "Starting a New Game with this slot will override the currently saved data. Are you sure?",
                // function to execute if we select 'yes'
                () =>
                {
                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                    DataPersistenceManager.instance.NewGame();
                    SaveGameAndLoadScene();
                },
                // function to execute if we select 'cancel'
                () =>
                {
                    this.ActivateMenu(isLoadingGame);
                }
            );
        }
    }

    //needs code for new save file in saveslotmenu

    private void SaveGameAndLoadScene()
    {
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // load the scene
        SceneManager.LoadSceneAsync(startSceneName);
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmationPopupMenu.ActivateMenu(
            "정말 삭제하시겠습니까?",
            // function to execute if we select 'yes'
            () =>
            {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());

                // 리스트에서 해당 슬롯 제거
                saveSlots.Remove(saveSlot);
                Destroy(saveSlot.gameObject);

                ActivateMenu(isLoadingGame);
            },
            // function to execute if we select 'cancel'
            () =>
            {
                ActivateMenu(isLoadingGame);
            }
        );
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }
    public void ActivateMenu(bool isLoadingGame)
    {
        // Set this menu to be active
        savePanel.OpenWindow();

        // Set mode
        this.isLoadingGame = isLoadingGame;

        // Load all profiles' game data
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        Debug.Log("Profiles found: " + profilesGameData.Count);

        // Enable back button
        backButton.interactable = true;

        // Clear any existing slots before populating new ones
        foreach (Transform child in saveSlotContainer)
        {
            Destroy(child.gameObject);
        }
        saveSlots.Clear();

        // Create save slots dynamically based on the profiles' data
        GameObject firstSelected = backButton.gameObject;
        foreach (var profile in profilesGameData)
        {
            if (profile.Value == null)
            {
                continue;
            }

            GameObject newSlot = Instantiate(saveSlotPrefab, saveSlotContainer);
            SaveSlot saveSlot = newSlot.GetComponent<SaveSlot>();
            saveSlots.Add(saveSlot);

            GameData profileData = profile.Value;
            saveSlot.SetData(profileData);

            // Set up the save & clear slot button to call OnSaveSlotClicked when clicked
            saveSlot.GetSaveBtn().onClick.AddListener(() => OnSaveSlotClicked(saveSlot));
            saveSlot.GetClearBtn().onClick.AddListener(() => OnClearClicked(saveSlot));

            saveSlot.SetInteractable(true);
            if (firstSelected.Equals(backButton.gameObject))
            {
                firstSelected = saveSlot.gameObject;
            }

        }

        // Set the first selected button
        Button firstSelectedButton = saveSlots.Count > 0 ? saveSlots[0].GetSaveFileButton() : backButton;
        this.SetFirstSelected(firstSelectedButton);
    }


    public void DeactivateMenu()
    {
        savePanel.CloseWindow();
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}