using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    public PlayerMovement player;


    public Button openCraftMenu;
    public Button fizzyrocket;
    public Button weapon2;
    public Button weapon3;

    // Start is called before the first frame update
    void Start()
    {
        openCraftMenu.onClick.AddListener(EnableButtons);

        DisableButtons();

        fizzyrocket.onClick.AddListener(createFizzyRocket);
        weapon2.onClick.AddListener(createWeapon2);
        weapon3.onClick.AddListener(createWeapon3);
    }

    private void EnableButtons() {
        fizzyrocket.gameObject.SetActive(true);
        weapon2.gameObject.SetActive(true);
        weapon3.gameObject.SetActive(true);

        openCraftMenu.onClick.RemoveListener(EnableButtons);
        openCraftMenu.onClick.AddListener(DisableButtons);

    }

    private void DisableButtons() {
        openCraftMenu.onClick.AddListener(EnableButtons);

        fizzyrocket.gameObject.SetActive(false);
        weapon2.gameObject.SetActive(false);
        weapon3.gameObject.SetActive(false);
    }

    private void createFizzyRocket() {
        // check inventory
        string[] neededItems = new string[3] {"Empty bottle", "Soda", "Candy" };
        Dictionary<string, int> inventory = player.getInventory();

        bool haveEverything = true;
        string missing = "Missing: ";
        foreach (string item in neededItems) {
            if (!inventory.ContainsKey(item)) {
                haveEverything = false;
                missing += "\t" + item;
            }
        }
        if (!haveEverything) {
            Debug.Log("Craft failed. " + missing);
            
        }
        else {

            // Create item
            Debug.Log("Inventory sufficient. Creating Fizzy Rocket");
            Dictionary<string, int> available_gadgets = player.getGadgets();
            int new_num = 1;
            if (available_gadgets.ContainsKey("FizzyRocket")) {
                new_num += available_gadgets["FizzyRocket"];
            }
            player.setGadgetItem("FizzyRocket", new_num);

            // subtract from inventory
            foreach (string item in neededItems) {
                player.setInventoryItem(item, inventory[item] - 1);
            }
        
        }        
    }

    

    private void createWeapon2() {

    }
    private void createWeapon3() {
        
    }
}
