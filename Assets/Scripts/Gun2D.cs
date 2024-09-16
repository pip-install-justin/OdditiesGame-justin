using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Gun2D : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject FizzyRocketPrefab;
    public float bulletSpeed = 10;
    private PlayerMovement player;

    void Start() {
        player = GetComponent<PlayerMovement>();

    }
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            string selectedWeapon = "FizzyRocket";
            GameObject selectedPrefab = FizzyRocketPrefab;
            // make sure selectedWeapon is available in gadgets
            Dictionary<string, int> available_gadgets = player.getGadgets();
            if (available_gadgets.ContainsKey(selectedWeapon)) { 
                
                // remove one ammo
                player.setGadgetItem(selectedWeapon, available_gadgets[selectedWeapon] - 1);
                // get current direction for aim
                int[] player_direction = player.getFacingDirection();
                Debug.Log(string.Join(", ", player_direction));
                Vector3 bullet_direction = new Vector3(player_direction[0], player_direction[1], 0);
                
                
                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                

                //change bullet spawn Left/Right
                Debug.Log(bulletSpawnPoint.rotation);
                if (bullet_direction[0] == -1) {
                    bulletSpawnPoint.position = player.transform.position + new Vector3(-2f, 0f, 0f);
                    rotation = Quaternion.Euler(0, 0, 180);
                    
                }
                else if (bullet_direction[0] == 1) {
                    bulletSpawnPoint.position = player.transform.position + new Vector3(2f, 0f, 0f);
                    rotation = Quaternion.Euler(0, 0, 0);
                }

                // change rotation Up/Down
                if (bullet_direction[1] == -1) {
                    bulletSpawnPoint.position = player.transform.position + new Vector3(0f, -2f, 0f);
                    rotation = Quaternion.Euler(0, 0, -90);
                    
                }
                else if (bullet_direction[1] == 1) {
                    bulletSpawnPoint.position = player.transform.position + new Vector3(0f, 2f, 0f);
                    rotation = Quaternion.Euler(0, 0, 90);
                }



                
                // shoot ammo
                var bullet = Instantiate(selectedPrefab, bulletSpawnPoint.position, rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = bullet_direction * bulletSpeed;
            
            }
        }
    }
}