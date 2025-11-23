using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Type of interactable: NPC or Object")]
    public InteractionType interactionType;

    [Tooltip("Dialogue/message to show when interacting")]
    [TextArea(3, 10)]
    public string interactionMessage;

    [Tooltip("Does this interaction count toward game completion?")]
    public bool countsTowardCompletion = true;

    [Tooltip("Can only interact once?")]
    public bool oneTimeInteraction = true;

    [Header("Visual Feedback")]
    [Tooltip("UI prompt to show when player is near (e.g., 'Press E to interact')")]
    public GameObject interactionPrompt;

    [Tooltip("Optional highlight effect when player is near")]
    public GameObject highlightEffect;

    // Internal state
    private bool playerInRange = false;
    private bool hasInteracted = false;

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
        // Check for E key press when player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Check if already interacted and it's one-time only
            if (oneTimeInteraction && hasInteracted)
                return;

            Interact();
        }
    }

    void Interact()
    {
        hasInteracted = true;

        // Show dialogue/message
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogue(interactionMessage, interactionType);
        }

        // Register interaction with GameManager
        if (countsTowardCompletion && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterInteraction();
        }

        // Hide prompt after interaction (if one-time)
        if (oneTimeInteraction && interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // Optional: Disable highlight after interaction
        if (oneTimeInteraction && highlightEffect != null)
        {
            highlightEffect.SetActive(false);
        }

        Debug.Log($"Interacted with {gameObject.name}: {interactionMessage}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player entered trigger zone
        if (other.CompareTag("Player") || other.name == "PlayerBody")
        {
            playerInRange = true;

            // Show interaction prompt
            if (interactionPrompt != null && (!oneTimeInteraction || !hasInteracted))
            {
                interactionPrompt.SetActive(true);
            }

            // Show highlight
            if (highlightEffect != null && (!oneTimeInteraction || !hasInteracted))
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