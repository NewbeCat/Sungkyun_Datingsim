using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] public Button firstSelected;

    public virtual void OnEnable()
    {
        SetFirstSelected(firstSelected);
    }

    public void SetFirstSelected(Button firstSelectedButton)
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
    }
}
