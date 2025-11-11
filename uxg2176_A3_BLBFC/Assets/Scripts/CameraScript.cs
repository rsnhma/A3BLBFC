using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    // ─────────────────────────────
    // 🎯 PURPOSE:
    // Multi-mode player camera:
    // [1] First-Person — inside player head
    // [2] Fixed Third-Person — stays behind player’s back
    // [3] Free Third-Person — mouse orbit view
    // ─────────────────────────────

    [Header("Target")]
    [Tooltip("The object this camera follows (usually the player).")]
    public Transform target;

    [Header("Free Look Settings (3rd Person)")]
    public float distance = 4f;
    public float height = 2f;
    public float rotationSpeed = 3f;
    public float smoothTime = 0.12f;

    [Header("Fixed Third-Person Settings")]
    public float fixedBackDistance = 6f;
    public float fixedHeight = 2.2f;
    public float fixedShoulderOffset = 0.6f;

    [Header("First-Person Settings")]
    [Tooltip("Offset of camera from player origin (head position).")]
    public Vector3 firstPersonOffset = new Vector3(0f, 1.7f, 0.15f);

    [Header("Look Target Height (for all modes)")]
    public float lookAtHeight = 1.5f;

    // ─────────────────────────────
    // Internal states
    // ─────────────────────────────
    private float yaw = 0f;
    private float pitch = 10f;
    private Vector3 currentVelocity = Vector3.zero;

    private enum CamMode { FirstPerson, FixedThird, FreeThird }
    private CamMode currentMode = CamMode.FreeThird;

    // ─────────────────────────────
    // 🕹️ Handle key input for mode switching
    // ─────────────────────────────
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = CamMode.FirstPerson;
            AlignYawToPlayer();
            Debug.Log("Camera: FIRST-PERSON mode");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMode = CamMode.FixedThird;
            AlignYawToPlayer();
            Debug.Log("Camera: FIXED THIRD-PERSON mode");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMode = CamMode.FreeThird;
            Debug.Log("Camera: FREE THIRD-PERSON mode");
        }
    }

    // ─────────────────────────────
    // Align yaw to player when switching modes
    // so camera direction matches player facing
    // ─────────────────────────────
    void AlignYawToPlayer()
    {
        if (target) yaw = target.eulerAngles.y;
    }

    // ─────────────────────────────
    // 📸 MAIN CAMERA UPDATE
    // ─────────────────────────────
    void LateUpdate()
    {
        if (!target) return;

        switch (currentMode)
        {
            case CamMode.FirstPerson: FirstPersonView(); break;
            case CamMode.FixedThird: FixedAngleView(); break;
            case CamMode.FreeThird: FreeLookView(); break;
        }
    }

    // ─────────────────────────────
    // 👁️ FIRST-PERSON VIEW
    // Camera sits at player's head and rotates with player
    // ─────────────────────────────
    void FirstPersonView()
    {
        // Desired position is player's local head offset
        Vector3 desiredPosition = target.position
                                + target.TransformDirection(firstPersonOffset);

        // Snap to position (you can SmoothDamp if desired)
        transform.position = desiredPosition;

        // Look straight ahead in player’s facing direction
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        // Optional: match player's forward direction exactly
        transform.rotation = target.rotation;
    }

    // ─────────────────────────────
    // 🎮 FIXED THIRD-PERSON (always behind player)
    // ─────────────────────────────
    void FixedAngleView()
    {
        Vector3 localBack = -target.forward * fixedBackDistance;
        Vector3 localUp = Vector3.up * fixedHeight;
        Vector3 localShould = target.right * fixedShoulderOffset;

        Vector3 desiredPosition = target.position + localBack + localUp + localShould;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothTime
        );

        transform.LookAt(target.position + Vector3.up * lookAtHeight);
    }

    // ─────────────────────────────
    // 🌀 FREE THIRD-PERSON (mouse orbit)
    // ─────────────────────────────
    void FreeLookView()
    {
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position
                                  - (rot * Vector3.forward * distance)
                                  + Vector3.up * height;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothTime
        );

        transform.LookAt(target.position + Vector3.up * lookAtHeight);
    }


}
