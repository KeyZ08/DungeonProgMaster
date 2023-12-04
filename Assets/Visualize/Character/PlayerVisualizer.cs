using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField] private CharacterRenderer chRenderer;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private float movementSpeed = 1f;

    private bool isMovement;

    public bool IsMovement => isMovement;

    public Vector2 Position => transform.position;

    public void Constructor(Vector2 spawnPosition)
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = spawnPosition;
        targetPosition = spawnPosition;
    }

    public void MoveTo(Vector2 target)
    {
        targetPosition = target;
        isMovement = true;
    }

    void FixedUpdate()
    {
        if (isMovement)
        {
            Vector2 currentPos = transform.position;
            Vector2 inputVector = targetPosition - (Vector2)transform.position;
            Vector2 movement = Vector2.ClampMagnitude(inputVector, movementSpeed * Time.fixedDeltaTime);
            Vector2 newPos = currentPos + movement;
            chRenderer.SetDirection(movement);
            rb.MovePosition(newPos); 
            if (movement.magnitude < .01f)
                isMovement = false;
        }
    }
}
