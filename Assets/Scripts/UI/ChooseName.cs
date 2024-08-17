using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChooseName : Menu
{
    [Header("input Name")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private GameObject alarm;
    [SerializeField] private string playerName = "플레이어";

    [Header("startGame?")]
    [SerializeField] private bool startGame = false;
    [SerializeField] private string startGameScene = "BaseScene";

    [Header("Panel")]
    [SerializeField] private OpenCloseWindow namePanelOpen;

    public void AwakePanel()
    {
        namePanelOpen.OpenWindow();
        playerNameInput.text = playerName;
        alarm.SetActive(false);
    }

    //마우스
    public void InputName()
    {
        if (playerNameInput.text.Length <= 0)
        {
            alarm.SetActive(true);
        }
        else
        {
            DataPersistenceManager.instance.NewGame();
            playerName = playerNameInput.text;
            DataPersistenceManager.instance.SaveGame();
            StartCoroutine(SaveandMove());
        }
    }

    private IEnumerator SaveandMove()
    {
        if (startGame)
        {
            DataPersistenceManager.instance.NewGame();
        }

        playerName = playerNameInput.text;

        if (startGame)
        {
            DataPersistenceManager.instance.SaveGame();
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadSceneAsync(startGameScene);
        }

        namePanelOpen.CloseWindow();
    }

    public void SaveData(ref GameData data)
    {
        data.PlayerName = this.playerName;
    }
}

