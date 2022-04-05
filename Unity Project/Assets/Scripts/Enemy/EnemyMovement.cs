using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    
    private Vector3 initialPosition;
    

    private void Start()
    {
        initialPosition = transform.position; 
    }


    // Update is called once per frame
    void Update()
    {
            float step = moveSpeed * Time.deltaTime;

                if (target != null)
                {
                    transform.LookAt(target);

                    // Keep current y value no matter what
                    Vector3 desiredPos = new Vector3(target.position.x, transform.position.y, target.position.z);

                    transform.position = Vector3.MoveTowards(transform.position, desiredPos, step);
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