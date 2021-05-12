using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Transform playerCameraTransform = null;

    public override void OnStartAuthority()
    {
        playerCameraTransform.gameObject.SetActive(true);
    }
}
