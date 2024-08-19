using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string profileID;
    public long lastUpdated;
    public string PlayerName;
    public Vector3 playerPosition;

    //public SerializableDict<--, --> should be used instead of a regular Dictionary.

    public GameData()
    {
        lastUpdated = System.DateTime.Now.ToBinary();
        this.PlayerName = "플레이어";
        playerPosition = Vector3.zero;
    }
}
