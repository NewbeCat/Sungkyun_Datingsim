using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mainmenu : Menu
{
    [SerializeField] private OpenCloseWindow SaveSystem;
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private OptionsBtn OptionPanelScript;

    [SerializeField] private string startGameScene;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionGameButton;
    [SerializeField] private Button endGameButton;

    private void Start()
    {
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }
        SetFirstSelected(firstSelected);
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        optionGameButton.interactable = false;
        endGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void StartBtn()
    {
        DeactivateMenu();
        SceneManager.LoadSceneAsync(startGameScene);
    }

    public void ContinueBtn()
    {
        DeactivateMenu();
        saveSlotsMenu.ActivateMenu(true);
    }

    public void OnOptionBtn()
    {
        DeactivateMenu();
        OptionPanelScript.OpenOption();
    }

    public void EndBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
