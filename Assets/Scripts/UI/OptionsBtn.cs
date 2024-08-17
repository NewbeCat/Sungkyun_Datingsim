using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsBtn : Menu
{
    [SerializeField] private OpenCloseWindow OptionOpen;
    [SerializeField] private GameObject pastSelectedBtn;
    [SerializeField] private GameObject firstSelectedBtn;

    [SerializeField] private OpenCloseWindow KeyOpen;
    [SerializeField] private OpenCloseWindow AudioOpen;
    [SerializeField] private OpenCloseWindow ScreenOpen;
    [SerializeField] private OpenCloseWindow GameSettOpen;

    public OpenCloseWindow CurrentOpen;
    private bool changing = false;

    public void OffOptionBtn()
    {
        StartCoroutine(OffOption());
    }

    private IEnumerator OffOption()
    {
        CurrentOpen.CloseWindow();
        CurrentOpen = null;
        yield return new WaitForSeconds(0.3f);
        EventSystem.current.SetSelectedGameObject(null);  // 기존 선택 초기화
        EventSystem.current.SetSelectedGameObject(pastSelectedBtn);  // 새로 선택할 버튼 설정
        OptionOpen.CloseWindow();
    }

    public void OpenOption()
    {
        OptionOpen.OpenWindow();
        KeyOptionOpen();
        EventSystem.current.SetSelectedGameObject(null);  // 기존 선택 초기화
        EventSystem.current.SetSelectedGameObject(firstSelectedBtn);  // 새로 선택할 버튼 설정
    }

    public void KeyOptionOpen()
    {
        if (!changing)
            StartCoroutine(ChangePanel(KeyOpen));
    }
    public void AudioOptionOpen()
    {
        if (!changing)
            StartCoroutine(ChangePanel(AudioOpen));
    }
    public void ScreenOptionOpen()
    {
        if (!changing)
            StartCoroutine(ChangePanel(ScreenOpen));
    }
    public void GameSettOptionOpen()
    {
        if (!changing)
            StartCoroutine(ChangePanel(GameSettOpen));
    }

    private IEnumerator ChangePanel(OpenCloseWindow selected)
    {
        if (!changing && CurrentOpen != selected)
        {
            changing = true;

            if (CurrentOpen != null)
                CurrentOpen.CloseWindow();
            yield return new WaitForSeconds(0.3f);
            selected.OpenWindow();
            CurrentOpen = selected;
            yield return new WaitForSeconds(0.3f);
            changing = false;
        }
    }
}
