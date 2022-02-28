using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Transform cameraPos;

    public static Rigidbody playerRB;
    public static bool moving = false;
    public Vector3 jump;
    public float jumpForce = 5.0f;

    public bool isGrounded;
    public float resetTime = 0f;
    public bool isReset = false;

    void Start()
    {
        cameraPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        playerRB = this.GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 5.0f, 0.0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void MovePlayer()
    {
        float side = Input.GetAxis("Horizontal");
        float fb = Input.GetAxis("Vertical");

        Vector3 vector = fb * transform.forward * Time.deltaTime * moveSpeed;
        Vector3 rotation = new Vector3(0, side, 0) * Time.deltaTime * Mathf.Pow(moveSpeed, 2);

        playerRB.MovePosition(transform.position + vector);
        playerRB.MoveRotation(transform.rotation * Quaternion.Euler(rotation));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            playerRB.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = moveSpeed * 2f;
            isReset = true;
        }

        if (isReset)
        {
            if (resetTime <= 3)
            {
                resetTime += Time.deltaTime;
            }
            if (resetTime > 3)
            {
                moveSpeed = 10f;
                isReset = false;
                resetTime = 0;
            }
        }
    }
}
