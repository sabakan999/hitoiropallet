using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReset : MonoBehaviour
{
    [SerializeField]
    private float inputDelay = 0.2f;

    private void Start()
    {
        ResetAndDisable(Gamepad.current);
        ResetAndDisable(Keyboard.current);
        ResetAndDisable(Mouse.current);

        StartCoroutine(EnableDevices());
    }

    private void ResetAndDisable(InputDevice device)
    {
        if (device == null)
        {
            return;
        }

        InputSystem.ResetDevice(device);
        InputSystem.DisableDevice(device);
    }

    private IEnumerator EnableDevices()
    {
        yield return new WaitForSecondsRealtime(inputDelay);

        Enable(Gamepad.current);
        Enable(Keyboard.current);
        Enable(Mouse.current);
    }

    private void Enable(InputDevice device)
    {
        if (device != null)
        {
            InputSystem.EnableDevice(device);
        }
    }
}