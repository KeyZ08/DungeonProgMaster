using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField] private CharacterRenderer chRenderer;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Direction targetDiretion;
    private float movementSpeed = 1f;

    private bool isRotated;
    private float rotatedTimer;
    private readonly float rotatedTimerDefault = 0.15f;//seconds
    private bool isMovement;

    public bool IsAnimated => isMovement || isRotated;

    public Vector2 Position => transform.position;

    public void Constructor(Vector2 spawnPosition, Direction spawnDirection)
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = spawnPosition;
        targetPosition = spawnPosition;
        TurnTo(spawnDirection);
        rotatedTimer = rotatedTimerDefault;
    }

    public void TurnTo(Direction direction)
    {
        targetDiretion = direction;
        isRotated = true;
    }

    public void MoveTo(Vector2 target)
    {
        targetPosition = target;
        isMovement = true;
    }

    void FixedUpdate()
    {
        if (isRotated)
        {
            rotatedTimer -= Time.fixedDeltaTime;
            if (rotatedTimer < 0)
            {
                chRenderer.SetDirection(targetDiretion);
                isRotated = false;
                rotatedTimer = rotatedTimerDefault;
            }
        }

        if (isMovement)
        {
            Vector2 currentPos = transform.position;
            Vector2 inputVector = targetPosition - (Vector2)transform.position;
            Vector2 movement = Vector2.ClampMagnitude(inputVector, movementSpeed * Time.fixedDeltaTime);
            Vector2 newPos = currentPos + movement;
            rb.MovePosition(newPos);
            if (movement.magnitude < .01f)
            {
                isMovement = false;
            }
        }
        chRenderer.PlayAnim(isMovement);
    }
}
