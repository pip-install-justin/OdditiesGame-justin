using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public SpriteRenderer dialogueBox; // Assign your dialogue box SpriteRenderer
    public Sprite[] dialogueSprites; // Assign your dialogue sprites in order

    private int currentSpriteIndex = 0;

    void Start()
    {
        if(dialogueSprites.Length > 0)
        {
        dialogueBox.sprite = dialogueSprites[currentSpriteIndex]; // Initialize the first sprite
        }
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
            dialogueBox.gameObject.SetActive(false); // Hide dialogue box when dialogue is over
        }
    }

    public void ResetDialogue()
    {
        currentSpriteIndex = 0;
        if(dialogueSprites.Length > 0)
        {
            dialogueBox.sprite = dialogueSprites[currentSpriteIndex]; // Set the sprite back to the first sprite
        }
    }
}
