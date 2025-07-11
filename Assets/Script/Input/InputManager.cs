using System;
using UnityEngine.InputSystem;

public class InputManager
{
    private PlayerControls playerControls;

    public float Movement => playerControls.gameplay.Movement.ReadValue<float>();

    public event Action OnJump;

    public InputManager()
    {
        playerControls = new PlayerControls();
        playerControls.gameplay.Enable();

        playerControls.gameplay.Jump.performed += OnJumpPerformed;


    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }
}
