using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [Tooltip("Panel containing dialogue UI")]
    public GameObject dialoguePanel;

    [Tooltip("Text component for dialogue")]
    public TextMeshProUGUI dialogueText;

    [Tooltip("Optional: NPC name display")]
    public TextMeshProUGUI npcNameText;

    [Tooltip("Button to close dialogue")]
    public Button closeButton;

    [Header("Settings")]
    [Tooltip("Auto-close dialogue after X seconds (0 = manual close)")]
    public float autoCloseDelay = 0f;

    private bool isDialogueActive = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Hide dialogue panel at start
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // Setup close button
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseDialogue);
        }
    }

    void Update()
    {
        // Allow pressing E or Escape to close dialogue
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseDialogue();
        }
    }

    public void ShowDialogue(string message, InteractableObject.InteractionType type)
    {
        if (dialoguePanel == null) return;

        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        // Set dialogue text
        if (dialogueText != null)
        {
            dialogueText.text = message;
        }

        // Set NPC name based on type (optional)
        if (npcNameText != null)
        {
            switch (type)
            {
                case InteractableObject.InteractionType.NPC:
                    npcNameText.text = "NPC";
                    break;
                case InteractableObject.InteractionType.Object:
                    npcNameText.text = "Clue";
                    break;
                default:
                    npcNameText.text = "";
                    break;
            }
        }

        // Auto-close if delay is set
        if (autoCloseDelay > 0)
        {
            CancelInvoke(nameof(CloseDialogue));
            Invoke(nameof(CloseDialogue), autoCloseDelay);
        }
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        isDialogueActive = false;
        CancelInvoke(nameof(CloseDialogue));
    }
}