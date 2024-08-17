using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileID = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playtime;

    [Header("ClearDataBtn")]
    [SerializeField] private Button clearBtn;

    public bool hasData { get; private set; } = false;
    [SerializeField] private Button saveSlotBtn;
    public void SetData(GameData data)
    {
        // there's no data for this profileId
        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearBtn.gameObject.SetActive(false);
        }
        // there is data for this profileId
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearBtn.gameObject.SetActive(true);

            playerName.text = data.PlayerName;
            playtime.text = "0";
        }
    }
    public string GetProfileId()
    {
        return this.profileID;
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