using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [Tooltip("Is the door locked at start?")]
    public bool isLocked = true;

    [Tooltip("Message when door is locked")]
    public string lockedMessage = "The door is locked. Complete all tasks first!";

    [Tooltip("Message when door is unlocked")]
    public string unlockedMessage = "Press E to exit";

    [Header("Visual Feedback")]
    public GameObject lockedIndicator;   // Red light, lock icon, etc.
    public GameObject unlockedIndicator; // Green light, open icon, etc.
    public GameObject interactionPrompt;

    private bool playerInRange = false;

    void Start()
    {
        UpdateDoorVisuals();

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isLocked)
            {
                // Player can exit - trigger level complete
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LevelComplete();
                }
            }
            else
            {
                // Show locked message
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.ShowDialogue(lockedMessage, InteractableObject.InteractionType.Door);
                }
            }
        }
    }

    // This gets called automatically when the door GameObject is activated
    void OnEnable()
    {
        UnlockDoor();
    }

    // Call this from GameManager when all interactions are complete
    public void UnlockDoor()
    {
        isLocked = false;
        UpdateDoorVisuals();

        // Optional: Show unlock message
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogue("All tasks complete! The door is now unlocked!", InteractableObject.InteractionType.Door);
        }

        Debug.Log("Door unlocked!");
    }

    void UpdateDoorVisuals()
    {
        if (lockedIndicator != null)
            lockedIndicator.SetActive(isLocked);

        if (unlockedIndicator != null)
            unlockedIndicator.SetActive(!isLocked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name == "PlayerBody")
        {
            playerInRange = true;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name == "PlayerBody")
        {
            playerInRange = false;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
}