using System.Collections;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject continueBox;

    [Header("Vitesse du texte")]
    public float wordSpeed;

    [Header("Touches de dialogue")]
    public KeyCode interactKey;
    public KeyCode passKey;

    [Header("Lignes de Dialogues")]
    public string[] dialogue;

    private int index = 0;
    private bool playerIsClose;
    private Coroutine dialogueTyping;

    void Start()
    {
        dialogueText.text = "";
    }

    void Update()
    {
        if (dialogueText.text == dialogue[index]){
                continueBox.SetActive(true);
            } else {
                continueBox.SetActive(false);
            }

        if (playerIsClose)
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (!dialoguePanel.activeInHierarchy)
                {
                    dialoguePanel.SetActive(true);
                    dialogueTyping = StartCoroutine(Typing());
                    Globals.InMenu = true;
                }
                else if (dialogueText.text == dialogue[index])
                {
                    NextLine();
                }
            }

            if (Input.GetKeyDown(passKey) && dialoguePanel.activeInHierarchy)
            {
                if (dialogueTyping != null)
                {
                    StopCoroutine(dialogueTyping);
                    dialogueTyping = null;
                }
                dialogueText.text = dialogue[index];
            }
        }
        else
        {
            RemoveText();
        }
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        Globals.InMenu = false;
    }

    IEnumerator Typing()
    {
        for (int i = 0; i < dialogue[index].Length; i++)
        {
            dialogueText.text += dialogue[index][i];
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            dialogueTyping = StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            RemoveText();
        }
    }
}