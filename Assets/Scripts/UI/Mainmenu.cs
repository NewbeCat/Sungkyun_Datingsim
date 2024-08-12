using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] private OpenCloseWindow Options;
    [SerializeField] private OptionsBtn OptionPanelScript;

    public void StartBtn()
    {
        Debug.Log("StartsGame");
    }

    public void ContinueBtn()
    {
        Debug.Log("OpensSaveSystem");
    }

    public void OnOptionBtn()
    {
        Options.OpenWindow();
        OptionPanelScript.KeyOptionOpen();
    }

    public void EndBtn()
    {
        Debug.Log("EndGame");
    }
}
