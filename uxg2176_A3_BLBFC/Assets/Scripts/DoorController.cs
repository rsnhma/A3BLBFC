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
            Debug.Log("E pressed at door! Locked: " + isLocked); // ADD THIS

            if (!isLocked)
            {
                Debug.Log("Door unlocked - calling LevelComplete"); // ADD THIS

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LevelComplete();
                }
            }
            else
            {
                Debug.Log("Door locked - showing message"); // ADD THIS

                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.ShowDialogue(lockedMessage, InteractableObject.InteractionType.Door, "");
                }
            }
        }
    }
 
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
        Debug.Log("Door trigger entered by: " + other.name + " | Tag: " + other.tag); // ADD THIS

        if (other.CompareTag("Player") || other.name == "PlayerBody")
        {
            Debug.Log("PLAYER DETECTED AT DOOR!"); // ADD THIS
            playerInRange = true;

            if (interactionPrompt != null)
            {
                Debug.Log("Showing interaction prompt"); // ADD THIS
                interactionPrompt.SetActive(true);
            }
            else
            {
                Debug.LogError("Interaction prompt is NULL! Not assigned in Inspector!"); // ADD THIS
            }
        }
        else
        {
            Debug.Log("Not the player - ignoring"); // ADD THIS
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