using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputSystem_Actions InputActions;

    public static InputSystem_Actions Controls
    {
        get
        {
            if (InputActions == null)
            {
                InputActions = new InputSystem_Actions();
            }

            return InputActions;
        }
    }
}
