using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using System;

public class PlayerController : MonoBehaviour
{
  
    
    public enum ACTIONS
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        JUMP
    }

    public ControllerMapping.CONTROLLERS controller = ControllerMapping.CONTROLLERS.KEYBOARD_1;

    private Dictionary<ACTIONS, InputEntry> _actions;
    private Dictionary<ACTIONS, bool> _state;

    private Array enumList;

    public bool LEFT
    {
        get { return pressed(ACTIONS.LEFT); }
    }
    public bool RIGHT
    {
        get { return pressed(ACTIONS.RIGHT); }
    }
    public bool UP
    {
        get { return pressed(ACTIONS.UP); }
    }
    public bool DOWN
    {
        get { return pressed(ACTIONS.DOWN); }
    }
    public bool JUMP
    {
        get { return pressed(ACTIONS.JUMP); }
    }

    public int gamepadNum = 0;

    // Use this for initialization
    void Start()
    {
        _actions = new Dictionary<ACTIONS, InputEntry>();
        _state = new Dictionary<ACTIONS, bool>();
        enumList = Enum.GetValues(typeof(ACTIONS));

        InputEntry obj;
        foreach (ACTIONS val in enumList)
        {
            obj = new InputEntry();
            obj.current = obj.last = 0;
            _actions.Add(val, obj);
            _state.Add(val, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        initState();

        #region GAMEPADS

        if (controller == ControllerMapping.CONTROLLERS.GAMEPAD_1) { gamepadNum = 0; }
        else if (controller == ControllerMapping.CONTROLLERS.GAMEPAD_2) { gamepadNum = -1; }
        else
        {
            gamepadNum = -1;

        }
        
        if (gamepadNum >= 0)
        {
            var inputDevice = (InputManager.Devices.Count > gamepadNum) ? InputManager.Devices[gamepadNum] : null;
            if (inputDevice != null)
            {
                processGamepad(inputDevice);
            }
            else
            {
                //Debug.Log("No Gamepad found");
            }
        }

        #endregion

        # region KEYBOARDS

        if (controller == ControllerMapping.CONTROLLERS.KEYBOARD_1 || controller == ControllerMapping.CONTROLLERS.KEYBOARD_2)
        {
            processKeyboard();
        }

        #endregion

        foreach (ACTIONS val in enumList)
        {
            handleKeyAction(val, _state[val]);
        }
    }

    void LateUpdate()
    {
        updateInput();
    }

    public bool pressed(ACTIONS action)
    {
        return _actions[action].current > 0;
    }

    public bool justPressed(ACTIONS action)
    {
        return _actions[action].current == 2;
    }

    void initState()
    {
        foreach (ACTIONS val in enumList)
        {
            _state[val] = false;
        }
    }

    void updateInput()
    {
        // clear all keys
        InputEntry o;
        foreach (ACTIONS key in enumList)
        {
            o = _actions[key];
            if ((o.last == -1) && (o.current == -1)) o.current = 0;
            else if ((o.last == 2) && (o.current == 2)) o.current = 1;
            o.last = o.current;
        }
    }

    void handleKeyDown(ACTIONS key)
    {
        if (!_actions.ContainsKey(key)) return;

        InputEntry obj = _actions[key];
        if (obj.current > 0) obj.current = 1;
        else obj.current = 2;
    }

    void handleKeyUp(ACTIONS key)
    {
        if (!_actions.ContainsKey(key)) return;

        InputEntry obj = _actions[key];
        if (obj.current > 0) obj.current = -1;
        else obj.current = 0;
    }

    void handleKeyAction(ACTIONS key, bool active)
    {
        if (!_actions.ContainsKey(key)) return;

        // there was no key down event and the key is currently pressed
        if ((!active) && pressed(key))
        {
            //Debug.Log(key + " was released");
            handleKeyUp(key); // set it as released
        }
        // the was a keydown event and the key isn't currently pressed
        else if (active && !pressed(key))
        {
            //Debug.Log(key + " was pressed");
            handleKeyDown(key);
        }
    }

    void processGamepad(InputDevice inputDevice)
    {
        _state[ACTIONS.JUMP] |= inputDevice.Action1.IsPressed;
        _state[ACTIONS.LEFT] |= inputDevice.Direction.Left;
        _state[ACTIONS.RIGHT] |= inputDevice.Direction.Right;
        _state[ACTIONS.DOWN] |= inputDevice.Direction.Down;
        _state[ACTIONS.UP] |= inputDevice.Direction.Up;
    }

    void processKeyboard()
    {
        _state[ACTIONS.JUMP] |= Input.GetKey(ControllerMapping.Keyboards[controller].JUMP);
        _state[ACTIONS.UP] |= Input.GetKey(ControllerMapping.Keyboards[controller].UP);
        _state[ACTIONS.LEFT] |= Input.GetKey(ControllerMapping.Keyboards[controller].LEFT);
        _state[ACTIONS.RIGHT] |= Input.GetKey(ControllerMapping.Keyboards[controller].RIGHT);
        _state[ACTIONS.DOWN] |= Input.GetKey(ControllerMapping.Keyboards[controller].DOWN);
    }
}
