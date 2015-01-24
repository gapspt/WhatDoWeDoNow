using UnityEngine;
using System.Collections;

public class BaseKeyboard {

    public KeyCode JUMP = KeyCode.None;
    public KeyCode UP = KeyCode.None;
    public KeyCode LEFT = KeyCode.None;
    public KeyCode RIGHT = KeyCode.None;
    public KeyCode DOWN = KeyCode.None;

    public static BaseKeyboard createDefaultKeyboard1()
    {
        BaseKeyboard kb = new BaseKeyboard();

        kb.JUMP = KeyCode.UpArrow;
        kb.LEFT = KeyCode.LeftArrow;
        kb.RIGHT = KeyCode.RightArrow;
        kb.DOWN = KeyCode.DownArrow;
        return kb;
    }

    public static BaseKeyboard createDefaultKeyboard2()
    {
        BaseKeyboard kb = new BaseKeyboard();
        kb.JUMP = KeyCode.W;
        kb.LEFT = KeyCode.A;
        kb.RIGHT = KeyCode.D;
        kb.DOWN = KeyCode.S;
        return kb;
    }
}
