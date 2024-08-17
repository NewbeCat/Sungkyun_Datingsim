using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one set of Managers on Scene. Newest is destroyed.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
