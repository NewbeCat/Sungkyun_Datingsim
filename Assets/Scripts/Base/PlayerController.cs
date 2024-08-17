using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    private Vector3 movement;
    private Vector2 input;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask interactableLayer;

    private void Update()
    {
        ProcessInputs();
        Move();
    }

    private void ProcessInputs()
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

    private void Animate()
    {
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetBool("isMoving", true);
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            Debug.Log("상호작용 진행");
        }
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }


    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed * movement.x, moveSpeed * movement.y);
    }
}
