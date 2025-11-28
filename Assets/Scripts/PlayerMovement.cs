using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    private Vector2 playerMoveInput;
    private Rigidbody2D rb;
    private Animator Player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody2D no encontrado.");
            enabled = false;
            return;
        }

        if (Player == null)
        {
            Debug.LogWarning("PlayerMovement: Animator no encontrado.");
        }

        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (InputManager.Instance != null)
            InputManager.Instance.OnMove += HandleMove;
    }

    void HandleMove(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            input.y = 0;
            input.x = Mathf.Sign(input.x);
        }
        else if (Mathf.Abs(input.y) > 0)
        {
            input.x = 0;
            input.y = Mathf.Sign(input.y);
        }
        else
        {
            input = Vector2.zero;
        }

        playerMoveInput = input;

        if (Player != null)
        {
            Player.SetFloat("MoveX", input.x);
            Player.SetFloat("MoveY", input.y);
            Player.SetBool("IsMoving", input != Vector2.zero);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = playerMoveInput * moveSpeed;
    }

}
