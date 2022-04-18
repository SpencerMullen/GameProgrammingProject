using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    float attackTimer = 0f;
    public float attackDamage = 10f;
    
    private Vector3 initialPosition;
    
    private Animator anim;

    protected virtual void Start()
    {
        initialPosition = transform.position; 
        anim = GetComponent<Animator>();
        anim.SetBool("isWalk", true);
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        float distToTarget = Vector3.Distance(this.transform.position, target.transform.position);

        if(this.gameObject.GetComponent<EnemyHealth>().alive == true) {
            if(distToTarget >= 5) {
                float step = moveSpeed * Time.deltaTime;
                if (target != null)
                {
                    transform.LookAt(target);

                    // Keep current y value no matter what
                    Vector3 desiredPos = new Vector3(target.position.x, transform.position.y, target.position.z);

                    transform.position = Vector3.MoveTowards(transform.position, desiredPos, step);
                }
            }
            if(distToTarget <= 10) {    
                this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                attackTimer += Time.deltaTime;
                //Debug.Log("ATimer: " + attackTimer);
                if(attackTimer >= 8f) {
                    GameObject.FindGameObjectWithTag("Lab").GetComponent<LabHealth>().TakeDamage(attackDamage);
                    //Debug.Log("ATTACK");
                    attackTimer = 0;
                }
            }
            
        } else {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void ResetInitialPosition()
    {
        transform.position = initialPosition; 
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }
}