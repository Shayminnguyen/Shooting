using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }
    public KeyCode jumpButton;
    public KeyCode shootButton;

    public System.Action<Vector2> OnCursorMoved;
    public System.Action<float> OnInputXChanged;
    public System.Action OnJumpButtonPressed;
    public System.Action OnShootButtonPressed;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (OnInputXChanged != null) OnInputXChanged(Input.GetAxisRaw("Horizontal"));
        if (OnCursorMoved != null) OnCursorMoved(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetKeyDown(jumpButton) && OnJumpButtonPressed != null) OnJumpButtonPressed();
        if (Input.GetKeyDown(shootButton) && OnShootButtonPressed != null) OnShootButtonPressed();
    }
}
