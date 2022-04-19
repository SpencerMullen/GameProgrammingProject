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
    bool isJumping;
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
        input.y = moveDirection.y;

        if(_controller.isGrounded){
            //we can jump here
            moveDirection = input;
            if(Input.GetButton("Jump")){
                //jump it!
                isJumping = true;
                GunAnim.Instance.anim.SetInteger("Movement", 2);
                moveDirection.y = Mathf.Sqrt(2f * gravity * jumpHeight);
                // Debug.Log("JUMP");
            } else {
                //ground the object
                isJumping = false;
                GunAnim.Instance.anim.SetInteger("Movement", 0);
                moveDirection.y = 0.0f;
            }
        } else {
            // mid air
            GunAnim.Instance.anim.SetInteger("Movement", 2);
            moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime * airControl);
        }
        moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(input * Time.deltaTime);
        //Debug.Log(input.magnitude);
        if (input.magnitude < .9f)
        {

            GunAnim.Instance.anim.SetInteger("Movement", 0);
        } else if (!isJumping)
        {
            GunAnim.Instance.anim.SetInteger("Movement", 1);
        }
    }
}