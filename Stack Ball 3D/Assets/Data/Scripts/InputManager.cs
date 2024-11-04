using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; }


    [SerializeField] protected bool mouseButtonDown;
    public bool MouseButtonDown { get => mouseButtonDown; }

    [SerializeField]protected bool mouseButtonUp;
    public bool MouseButtonUp { get => mouseButtonUp; }

    [SerializeField]protected bool mouseButton;
    public bool MouseButton {  get => mouseButton; }

    private void Awake()
    {
        if (InputManager.instance != null) Debug.LogError("Only 1 InputManager ");
        InputManager.instance = this;
    }
    private void Update()
    {
        this.GetMouseDown();
        this.GetMouseUp();
        this.GetMouse();
    }
    protected virtual void GetMouseDown()
    {
        this.mouseButtonDown = Input.GetMouseButtonDown(0);
    }
    protected virtual void GetMouseUp()
    {
        this.mouseButtonUp = Input.GetMouseButtonUp(0);
    }
    protected virtual void GetMouse()
    {
        this.mouseButton = Input.GetMouseButton(0);
    }
    
}
