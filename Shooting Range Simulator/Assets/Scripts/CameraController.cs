using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 mouseCurrent;
    public float sensitivity = 2.0f;

    GameObject character;

    void Start()
    {
        mouseCurrent.x = 0;
        character = this.transform.parent.gameObject;
    }

    void Update()
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
