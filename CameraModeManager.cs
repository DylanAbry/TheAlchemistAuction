using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeManager : MonoBehaviour
{
    public static CameraModeManager Instance;

    [Header("Cameras")]
    public Camera playerCam;
    public Camera chessCam;

    [Header("Player")]
    public GameObject player;

    Vector3 savedPos;
    Quaternion savedRot;

    bool inChessMode = false;

    void Awake()
    {
        Instance = this;
    }

    public void EnterChessMode()
    {
        if (inChessMode) return;

        // Save player state
        savedPos = player.transform.position;
        savedRot = player.transform.rotation;

        // Disable player control
        player.GetComponent<PlayerMovement>().enabled = false;

        // Switch cams
        playerCam.gameObject.SetActive(false);
        chessCam.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inChessMode = true;
    }

    public void ExitChessMode()
    {
        if (!inChessMode) return;

        // Restore player
        player.transform.position = savedPos;
        player.transform.rotation = savedRot;

        player.GetComponent<PlayerMovement>().enabled = true;

        // Switch cams
        chessCam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inChessMode = false;
    }
}
