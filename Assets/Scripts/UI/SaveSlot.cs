using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] public string profileID = "";

    [Header("Content")]
    [SerializeField] public TextMeshProUGUI playerName;
    [SerializeField] public TextMeshProUGUI playtime;

    [Header("ClearDataBtn")]
    [SerializeField] private Button clearBtn;
    [SerializeField] private Button saveSlotBtn;
    public void SetData(GameData data)
    {
        // there's no data for this profileId
        if (data == null)
        {
            return;
        }
        // there is data for this profileId
        else
        {
            this.SetInteractable(true);
            profileID = data.profileID;
            playerName.text = data.PlayerName;
            playtime.text = "0";
        }
    }
    public string GetProfileId()
    {
        return this.profileID;
    }

    public Button GetSaveBtn()
    {
        return saveSlotBtn;
    }

    public Button GetClearBtn()
    {
        return clearBtn;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotBtn.interactable = interactable;
        clearBtn.interactable = interactable;
    }
    public Button GetSaveFileButton()
    {
        return saveSlotBtn;
    }
}