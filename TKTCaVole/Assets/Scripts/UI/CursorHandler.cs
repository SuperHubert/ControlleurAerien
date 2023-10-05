using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorHandler : MonoBehaviour
{
    private void Start()
    {
        return;
        GameInputManager.OnPausePerformed += OnPausePerformed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        Cursor.lockState = Cursor.lockState==CursorLockMode.Locked?CursorLockMode.None:CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }
}
