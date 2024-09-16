using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public Sprite[] dialogueSprites; // Assign your dialogue sprites in order
    private int currentSpriteIndex = 0;
    private SpriteRenderer dialogueBox;

    void Start()
    {
        dialogueBox = GetComponent<SpriteRenderer>();
        dialogueBox.sprite = dialogueSprites[currentSpriteIndex]; // Initialize the first sprite
        dialogueBox.enabled = false; // Hide the dialogue box at the start
    }

    public void DisplayNextSprite()
    {
        currentSpriteIndex++;

        if (currentSpriteIndex < dialogueSprites.Length)
        {
            dialogueBox.sprite = dialogueSprites[currentSpriteIndex];
        }
        else
        {
            EndDialogue(); // End dialogue when all sprites are shown
        }
    }

    public void StartDialogue()
    {
        dialogueBox.enabled = true; // Show the dialogue box
    }

    public void EndDialogue()
    {
        dialogueBox.enabled = false; // Hide the dialogue box
        currentSpriteIndex = 0;
        dialogueBox.sprite = dialogueSprites[currentSpriteIndex]; // Reset the sprite to the first one
    }
}
