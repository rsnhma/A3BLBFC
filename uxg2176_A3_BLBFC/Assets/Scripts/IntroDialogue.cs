using UnityEngine;

public class IntroDialogue : MonoBehaviour
{
    [Header("Intro Message")]
    [TextArea(3, 10)]
    [Tooltip("Message to show when game starts")]
    public string introMessage = "You are trapped in the supermarket! The exit door is locked. Find the NPC to figure out how to escape.";

    [Header("Settings")]
    [Tooltip("Delay before showing intro (in seconds)")]
    public float showDelay = 0.5f;

    private bool hasShownIntro = false;

    void Start()
    {
        // Show intro after a short delay
        Invoke(nameof(ShowIntro), showDelay);
    }

    void ShowIntro()
    {
        if (hasShownIntro) return;

        // Check if DialogueManager exists
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogue(introMessage, InteractableObject.InteractionType.NPC);
            hasShownIntro = true;
        }
        else
        {
            Debug.LogError("DialogueManager not found! Make sure DialogueManager exists in the scene.");
        }
    }
}