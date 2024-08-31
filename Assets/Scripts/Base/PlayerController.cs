using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    private Vector3 movement;
    private Vector2 input;
    private Vector3 facingDir;
    [SerializeField] private Vector3 interactPos;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask interactableLayer;

    private void Update()
    {
        ProcessInputs();
        Move();
    }

    private void ProcessInputs()
    {
        if (!DialogueManager.Instance.isDialogue)
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
            if (new Vector2(movement.x, movement.y) != Vector2.zero)
            {
                Animate();
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (movement.magnitude > 1.0f)
            {
                movement.Normalize();
            }

            if (Input.GetKeyDown(KeyCode.Z)) Interact();
        }
    }

    private void Animate()
    {
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        facingDir = setDirection(movement.x, movement.y);
        animator.SetBool("isMoving", true);
    }

    void Interact()
    {
        // 상호작용 위치 계산
        Debug.Log(transform.localPosition);
        interactPos = transform.localPosition + facingDir;
        Debug.Log("current scanner" + interactPos);

        // 상호작용 가능한 오브젝트를 확인
        float radius = 0.1f; // 반지름을 적절히 설정
        var collider = Physics2D.OverlapCircle(interactPos, radius, interactableLayer);

        // 결과 확인
        if (collider != null)
        {
            Debug.Log("상호작용 진행");
        }
        else
        {
            Debug.Log("상호작용 가능한 오브젝트 없음");
        }
    }

    private Vector3 setDirection(float x, float y)
    {
        if (x == 0)
        {
            if (y > 0)
            {
                return new Vector3(0f, 0f, 0f);
            }
            else
            {
                return new Vector3(0f, -0.5f, 0f);
            }
        }
        else
        {
            if (x > 0)
            {
                return new Vector3(0.2f, -0.25f, 0f);
            }
            else
            {
                return new Vector3(-0.2f, -0.25f, 0f);
            }
        }
    }


    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }


    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed * movement.x, moveSpeed * movement.y);
    }
}
