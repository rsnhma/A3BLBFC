using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Type of interactable: NPC or Object")]
    public InteractionType interactionType;

    [Tooltip("Name to display (e.g., 'Mike' for NPC, leave blank for 'Clue')")]
    public string speakerName = "";

    [Tooltip("Dialogue/message to show when interacting")]
    [TextArea(3, 10)]
    public string interactionMessage;

    [Tooltip("Does this interaction count toward game completion?")]
    public bool countsTowardCompletion = true;

    [Tooltip("Can only interact once?")]
    public bool oneTimeInteraction = false;

    [Header("Visual Feedback")]
    [Tooltip("UI prompt to show when player is near (e.g., 'Press E to interact')")]
    public GameObject interactionPrompt;

    [Tooltip("Optional highlight effect when player is near")]
    public GameObject highlightEffect;

    // Internal state
    private bool playerInRange = false;
    private bool hasInteracted = false;

    // Public method to check if this object was interacted with
    public bool HasInteracted()
    {
        return hasInteracted;
    }

    public enum InteractionType
    {
        NPC,
        Object,
        Door
    }

    void Start()
    {
        // Hide prompt at start
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (highlightEffect != null)
            highlightEffect.SetActive(false);
    }

    void Update()
    {
        // Don't allow interaction while dialogue is open
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
        {
            return;
        }

        // Check for E key press when player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        // Show dialogue/message with custom speaker name
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogue(interactionMessage, interactionType, speakerName);
        }

        // Register interaction with GameManager (only count once)
        // FIXED: Check hasInteracted BEFORE setting it to true!
        if (countsTowardCompletion && GameManager.Instance != null && !hasInteracted)
        {
            GameManager.Instance.RegisterInteraction();
        }

        hasInteracted = true; // Set AFTER counting

        Debug.Log($"Interacted with {gameObject.name}: {interactionMessage}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player entered trigger zone
        if (other.CompareTag("Player") || other.name == "PlayerBody")
        {
            playerInRange = true;

            // Show interaction prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }

            // Show highlight
            if (highlightEffect != null)
            {
                highlightEffect.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if player left trigger zone
        if (other.CompareTag("Player") || other.name == "PlayerBody")
        {
            playerInRange = false;

            // Hide interaction prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            // Hide highlight
            if (highlightEffect != null)
            {
                highlightEffect.SetActive(false);
            }
        }
    }
}