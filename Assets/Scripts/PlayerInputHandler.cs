using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var playerControllers = FindObjectsOfType<PlayerController>();
        var index = playerInput.playerIndex;
        playerController = playerControllers.FirstOrDefault(m => m.playerIndex == index);
    }

    public void OnMove(CallbackContext context)
    {
        Vector2 controllerDirection = context.ReadValue<Vector2>();
        playerController.SetPlayerMoveDirection(new Vector3(controllerDirection.x, 0, controllerDirection.y));
    }

    public void OnJump(CallbackContext context)
    {
        playerController.SetJumpPressed(true);
    }
}
