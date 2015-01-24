using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerMapping {

    public enum CONTROLLERS
    {
        GAMEPAD_1,
        GAMEPAD_2,
        KEYBOARD_1,
        KEYBOARD_2,
    }

    public static Dictionary<CONTROLLERS, BaseKeyboard> Keyboards;

    static ControllerMapping()
    {
        Keyboards = new Dictionary<CONTROLLERS, BaseKeyboard>();
        Keyboards.Add(CONTROLLERS.KEYBOARD_1, BaseKeyboard.createDefaultKeyboard1());
        Keyboards.Add(CONTROLLERS.KEYBOARD_2, BaseKeyboard.createDefaultKeyboard2());
    }
	
}
