using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class SU_CharacterController : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float peakTime = 0.3f;

    private InputAction jumpAction;
    private InputAction godModeAction;
    bool requestJump = false;

    private bool playerGrounded;
    private InputAction moveAction;

    private Vector2 acceleration;
    private Vector2 velocity;
    private float gravity;

    private Rigidbody2D rb;
    Collider2D collider;
    private ContactFilter2D contactFilter;


// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
        moveAction = InputSystem.actions.FindAction("Move");
        rb = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        //gameObject.transform.position = new Vector3(0f, ground, 0f);
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        contactFilter.useNormalAngle = true;
        contactFilter.minNormalAngle = 90;
        contactFilter.maxNormalAngle = 90;

        gravity = 2f * jumpHeight / (peakTime * peakTime);

    }


    // Update is called once per frame
    private void Update()
    {
        Jump(jumpAction);
    }


    private void Jump(InputAction jump)
    {
        if (jump.triggered && playerGrounded)
        {
            requestJump = true;
        }
    }

    private void TriggerJump(bool jumpTriggered)
    {
        if (!jumpTriggered) return;
        velocity.y = gravity * peakTime;
        playerGrounded = false;
        requestJump = false;
    }

    void ApplyGravity()
    {
        if (!playerGrounded)
        {
            // gravity is acceleration in this contex
            acceleration.y = -gravity;
            velocity += acceleration * Time.fixedDeltaTime;
        }
        else if (velocity.y <= 0)
        {
            velocity.y = 0;
        }
    }

    bool GroundCheck()
    {
        if (Physics2D.IsTouching(collider, contactFilter))
        {
            playerGrounded = true;
        }
        else
        {
            playerGrounded = false;
        }

        return playerGrounded;
    }

    void FixedUpdate()
    {
        acceleration = Vector2.zero;
        GroundCheck();
        ApplyGravity();
        TriggerJump(requestJump);
        acceleration.x = moveAction.ReadValue<Vector2>().x * 5;
        velocity.x = acceleration.x;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
