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

    private List<SaveSlot> saveSlots = new List<SaveSlot>();

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
        else if (saveSlot.hasData)
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
        // case - new game, and the save slot has no data
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            SaveGameAndLoadScene();
        }
    }

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
            "Are you sure you want to delete this saved data?",
            // function to execute if we select 'yes'
            () =>
            {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
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
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        backButton.interactable = true;

        // 먼저 기존에 생성된 슬롯들을 모두 삭제
        foreach (Transform child in saveSlotContainer)
        {
            Destroy(child.gameObject);
        }

        saveSlots.Clear(); // 리스트 초기화

        // loop through each save slot in the UI and set the content appropriately
        GameObject firstSelected = backButton.gameObject;
        foreach (var profile in profilesGameData)
        {
            GameObject newSlot = Instantiate(saveSlotPrefab, saveSlotContainer); // 슬롯을 동적으로 생성
            SaveSlot saveSlot = newSlot.GetComponent<SaveSlot>();
            saveSlots.Add(saveSlot);

            GameData profileData = profile.Value;
            saveSlot.SetData(profileData);

            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        // set the first selected button
        Button firstSelectedButton = saveSlots.Count > 0 ? saveSlots[0].GetSaveFileButton() : backButton;
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
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