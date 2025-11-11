using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    void Update()
    {
        // Move forward/backward
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        // Turn left/right
        float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Apply movement
        transform.Translate(0f, 0f, move);
        transform.Rotate(0f, turn, 0f);
    }
}
