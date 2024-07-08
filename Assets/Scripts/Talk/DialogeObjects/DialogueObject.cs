using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue/DialogueObject", order = 1)]
public class DialogueObject : ScriptableObject
{
    [System.Serializable]
    public class Dialogue
    {
        public DialogueOptions dialogueOptions;
        [TextArea]
        public string dialogueText;
        public string name;
        public DialogueTextStyle textStyle;
        public DialogueTextAnimation textAnimation;

        //options
        public ProfileOptions profileOptions;
        public OnePersonOptions onePersonOptions;
        public TwoPersonOptions twoPersonOptions;

    }

    public List<Dialogue> dialogue;
}

[System.Serializable]
public class ProfileOptions
{
    public FaceImage faceImage;
}

[System.Serializable]
public class OnePersonOptions
{
    public FaceImage faceImage;
    public ImagePlace imagePlace;
    public ImageEffect imageEffect;
}

[System.Serializable]
public class TwoPersonOptions
{
    public FaceImage faceA;
    public FaceImage faceB;
    public ImageEffect effectA;
    public ImageEffect effectB;
}


public enum DialogueOptions
{
    NoImg,
    Profile,
    OnePerson,
    TwoPerson,
}

public enum DialogueTextStyle
{
    Normal,
    Bold,
    Italic,
    // 필요에 따라 추가 스타일
}

public enum DialogueTextAnimation
{
    Default,
    WordWobble,
    AngryShake,
    // 필요에 따라 추가 스타일
}

public enum FaceImage
{
    Normal,
    Happy,
    Sad,
    Angry,
    Surprised,
}

public enum ImagePlace
{
    Left,
    Right,
    Middle,
}

public enum ImageEffect
{
    None,
    StepUp,
    StepDown,
    Jump,
}