using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    //public TextMeshProUGUI hqLabel;

    private Rigidbody rb;
    private Transform cam;
    private float cameraPitch = 0f;

    bool playerMove;

    void Start()
    {
        playerMove = false;
        StartCoroutine(PlayerMoveAllow());
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        //hqLabel.enabled = false;
    }

    private IEnumerator PlayerMoveAllow()
    {
        yield return new WaitForSeconds(0.75f);
        playerMove = true;
        //hqLabel.enabled = true;
    }

    void Update()
    {
        if (playerMove) HandleMouseLook();
    }

    void FixedUpdate()
    {
        if (playerMove) HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera (vertical)
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cam.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * inputX + transform.forward * inputZ;
        Vector3 targetVelocity = moveDirection * moveSpeed;
        Vector3 currentVelocity = rb.velocity;

        // Preserve current Y velocity (gravity, falling, etc.)
        rb.velocity = new Vector3(targetVelocity.x, currentVelocity.y, targetVelocity.z);
    }
}
