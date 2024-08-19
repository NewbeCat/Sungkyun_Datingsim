using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldNavigation : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public Button confirmButton;

    void Update()
    {
        // 입력 필드가 포커스된 상태이고, 오른쪽 방향키가 눌렸을 때
        if (nameInputField.isFocused && Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 커서가 텍스트의 끝에 위치해 있는지 확인
            if (nameInputField.caretPosition == nameInputField.text.Length)
            {
                // 확인 버튼으로 포커스 이동
                confirmButton.Select();
            }
        }
    }
}

