using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 mouseCurrent;
    public float sensitivity = 2.0f;
    private GameObject character;
    bool gamePlaying;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseCurrent.x = 0;
        character = this.transform.parent.gameObject;
        gamePlaying = true;
    }

    void FixedUpdate()
    {
        if (gamePlaying)
        {
            var mouseGet = new Vector2(Input.GetAxis("Mouse X") * sensitivity, Input.GetAxis("Mouse Y") * sensitivity);
            mouseCurrent += mouseGet;
            if (mouseCurrent.y >= 90)
                mouseCurrent.y = 90;
            else if (mouseCurrent.y <= -90)
                mouseCurrent.y = -90;
            transform.localRotation = Quaternion.AngleAxis(-mouseCurrent.y, Vector3.right);
            character.transform.localRotation = Quaternion.AngleAxis(mouseCurrent.x, character.transform.up);
        }
    }

    public void StopCam()
    {
        gamePlaying = false;
    }
    public void ResumeCam()
    {
        gamePlaying = true;
    }
}
