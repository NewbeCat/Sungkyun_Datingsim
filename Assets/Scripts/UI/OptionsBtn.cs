using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsBtn : MonoBehaviour
{
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private OpenCloseWindow OptionOpen;

    [SerializeField] private GameObject KeyPanel;
    [SerializeField] private OpenCloseWindow KeyOpen;
    [SerializeField] private GameObject AudioPanel;
    [SerializeField] private OpenCloseWindow AudioOpen;
    [SerializeField] private GameObject ScreenPanel;
    [SerializeField] private OpenCloseWindow ScreenOpen;
    [SerializeField] private GameObject GameSettPanel;
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
        OptionOpen.CloseWindow();
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
