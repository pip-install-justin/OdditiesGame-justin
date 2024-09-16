using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour {


    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public GameObject gun;

    private Dictionary<string, int> objectCounts = new Dictionary<string, int>();
    private Dictionary<string, int> gadgets = new Dictionary<string, int>();
    
    private int[] facing_direction = new int[2] {-1, 0};
   
	// Use this for initialization
	void Start () {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        // TODO remove
        gadgets["FizzyRocket"] = 10;

	}
	
	// Update is called once per frame
	void Update () {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        

        if(Input.GetButtonDown("attack") && currentState != PlayerState.attack 
           && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }

	}

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);

            facing_direction[0] = (int) change.x;
            facing_direction[1] = (int) change.y;

            // aim gun sprite
            if (change.x == -1) {
                gun.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (change.x == 1) {
                gun.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (change.y == -1) {
                gun.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            if (change.y == 1) {
                gun.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            gun.transform.position = transform.position + new Vector3((float) change.x, (float) change.y * 1.3f, 0f);

        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {

            StartCoroutine(KnockCo(knockTime));
        }else{
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;

        // add to inventory
        if (hitObject.CompareTag("Collectable"))
        {
            // Increase the count of this type of object
            string objectName = hitObject.name;
            if (objectCounts.ContainsKey(objectName))
            {
                objectCounts[objectName]++;
            }
            else
            {
                objectCounts[objectName] = 1;
            }

            // Destroy the object
            Destroy(hitObject);
        }
    }

    void OnGUI()
    {
        // Display the counts of all collected objects
        GUILayout.BeginArea(new Rect(500, 10, 700, 200));
        foreach (var pair in objectCounts)
        {
            GUILayout.Label(string.Format("{0}: {1}", pair.Key, pair.Value));
        }
        GUILayout.EndArea();

        // Display the counts of all gadgets
        GUILayout.BeginArea(new Rect(300, 10, 500, 200));
        foreach (var pair in gadgets)
        {
            GUIStyle customStyle = new GUIStyle(GUI.skin.label);
            int fontSize = 40;
            customStyle.fontSize = fontSize;
            string msg = string.Format("{0}: {1}", pair.Key, pair.Value);
            GUILayout.Label(string.Format(msg), customStyle);
        }
        GUILayout.EndArea();
    }

    public Dictionary<string, int> getInventory() {
        return objectCounts;
    }

    public Dictionary<string, int> getGadgets() {
        return gadgets;
    }

    public void setInventoryItem(string item, int new_num) {
        objectCounts[item] = new_num;
        if (objectCounts[item] == 0) {
            objectCounts.Remove(item);
        }
    }

    public void setGadgetItem(string item, int new_num) {
        gadgets[item] = new_num;
        if (gadgets[item] == 0) {
            gadgets.Remove(item);
        }
    }

    public int[] getFacingDirection() {
        return facing_direction;
    }

   

}
