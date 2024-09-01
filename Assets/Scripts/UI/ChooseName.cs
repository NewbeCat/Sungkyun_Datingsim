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
    [SerializeField] public string playerName = "플레이어";

    [Header("startGame?")]
    [SerializeField] private bool startGame = false;
    [SerializeField] private string startGameScene = "BaseScene";

    [Header("Panel")]
    [SerializeField] private OpenCloseWindow namePanelOpen;

    private void Awake()
    {
        StartCoroutine(AwakeRoutine());
    }

    private IEnumerator AwakeRoutine()
    {
        yield return null;
        namePanelOpen.OpenWindow();
        playerNameInput.text = DataPersistenceManager.instance.currentPlayerName;
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
            StartCoroutine(SaveandMove());
        }
    }

    private IEnumerator SaveandMove()
    {
        DataPersistenceManager.instance.currentPlayerName = playerNameInput.text;

        if (startGame)
        {
            DataPersistenceManager.instance.NewGame();
            namePanelOpen.CloseWindow();

            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadSceneAsync(startGameScene);
        }
        else
        {
            namePanelOpen.CloseWindow();
        }

    }
}