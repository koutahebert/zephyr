﻿using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 50;

    public Transform playerBody;
    private float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);

        transform.localEulerAngles = new Vector3(xRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
