using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyManager : MonoBehaviour
{
    public static InputKeyManager Instance;
    void Awake()
    {
        Instance = this;
    }
    [SerializeField] Action act_Mouse1 = null;
    [SerializeField] Action act_Mouse1Down = null;
    void Update()
    {
        if (Input.anyKey)
        {
            if (true)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keyCode))
                    {
                        switch (keyCode)
                        {
                            case KeyCode.W:
                                break;
                            case KeyCode.A:
                                break;
                            case KeyCode.S:
                                break;
                            case KeyCode.D:
                                break;
                            case KeyCode.Mouse1:
                                act_Mouse1?.Invoke();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        if (Input.anyKeyDown)
        {
            if (true)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        switch (keyCode)
                        {
                            case KeyCode.W:
                                break;
                            case KeyCode.A:
                                break;
                            case KeyCode.S:
                                break;
                            case KeyCode.D:
                                break;
                            case KeyCode.Mouse1:
                                act_Mouse1Down?.Invoke();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    public void AddMouse1Click(Action action)
    {
        act_Mouse1 += action;
    }
    public void AddMouse1DownClick(Action action)
    {
        act_Mouse1Down += action;
    }
}
