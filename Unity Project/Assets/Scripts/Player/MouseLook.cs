using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    public static float mouseSensitivity = 100f;
    float pitch = 0;
    float yaw = 0;

    void Awake(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(mouseSensitivity);
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //transform.Rotate(Vector3.up * moveX);

        yaw += moveX;
        pitch -= moveY;
        pitch = Mathf.Clamp(pitch, -75, 75);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);

    }
}
