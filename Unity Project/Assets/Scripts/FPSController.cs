using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float moveSpeed;
    public float gravity = 9.81f;

    public float jumpHeight = 5f;
    Vector3 input, moveDirection;
    CharacterController _controller;
    public float airControl = 10f;

    // Start is called before the first frame update
    void Awake()
    {

        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        input = transform.right * moveHorizontal + transform.forward * moveVertical;

        input *= moveSpeed;

        if(_controller.isGrounded){
            //we can jump here
            moveDirection = input;
            if(Input.GetButton("Jump")){
                //jump it!
                moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            }
            else {
                //ground the object
                moveDirection.y = 0.0f;
            }
        } else {
            
            // mid air
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime * airControl);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(input * Time.deltaTime);
    }
}