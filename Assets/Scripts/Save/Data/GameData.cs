using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string PlayerName;
    public Vector3 playerPosition;

    //public SerializableDict<--, --> should be used instead of a regular Dictionary.

    public GameData()
    {
        this.PlayerName = "플레이어";
        playerPosition = Vector3.zero;
    }
}
