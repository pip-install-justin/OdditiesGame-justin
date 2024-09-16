using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slob : Enemy {

    private Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    private float currentSpeed;
    public float acceleration;
    public Animator anim;

    //this enemy is like log but instead of constant speed accelerates towards player (has faster top speed)
	// Use this for initialization
	void Start () {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        currentSpeed = 0;
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CheckDistance();
	}

    void CheckDistance()
    {
        if(Vector3.Distance(target.position, 
                            transform.position) <= chaseRadius
           && Vector3.Distance(target.position,
                               transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger)
            {
                if (currentSpeed < moveSpeed)
                {
                    currentSpeed += Mathf.Min(acceleration * Time.deltaTime, 1);
                    Vector3 temp = Vector3.MoveTowards(transform.position,
                        target.position,
                        moveSpeed * currentSpeed * Time.deltaTime);
                    changeAnim(temp - transform.position);
                    myRigidbody.MovePosition(temp);
                    ChangeState(EnemyState.walk);
                }
                else
                {
                    Vector3 temp = Vector3.MoveTowards(transform.position,
                        target.position,
                        moveSpeed  * Time.deltaTime);
                    changeAnim(temp - transform.position);
                    myRigidbody.MovePosition(temp);
                    ChangeState(EnemyState.walk);
                }

                //Debug.Log(currentSpeed);
                anim.SetBool("wakeUp", true);
            }
        }else if (Vector3.Distance(target.position,
                            transform.position) > chaseRadius)
        {
            anim.SetBool("wakeUp", false);
            currentSpeed = 0;
        }
    }

    private void SetAnimFloat(Vector2 setVector){
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    private void changeAnim(Vector2 direction){
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0){
                SetAnimFloat(Vector2.right);
                print("right");
            }else 
            {
                print("left");
                SetAnimFloat(Vector2.left);
            }
        }
    }

    private void ChangeState(EnemyState newState){
        if(currentState != newState)
        {
            currentState = newState;
        }
    }
}
