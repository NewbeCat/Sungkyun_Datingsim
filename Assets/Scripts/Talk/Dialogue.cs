using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //기본
    [Tooltip("말하는 캐릭터 이름")]
    public string name;

    [Tooltip("대사")]
    public string[] contexts;

    [Tooltip("선택지")]
    public string[] choicenum;

    [Tooltip("스킵")]
    public string[] skipnum;

    //이미지
    [Tooltip("이미지")]
    public string[] imgname;

    //오디오 등 기타 이벤트 public string[] events;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;
    public Vector2 line;
    public Dialogue[] dialogues;
}
