using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class NPCController : MonoBehaviour
{
    private PlayerController controller;

    private bool isProcessingAI = false;

    // Thread safe variables (because the original variables cannot be accessed outside of the main thread)
    private bool tsEnabled;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update () {
        GetTrheadSafeVariables();

        if (!isProcessingAI)
        {
            GameManager.Enqueue(PerformAI);
        }
    }

    private void GetTrheadSafeVariables()
    {
        tsEnabled = enabled;
    }

    private void PerformAI()
    {
        if (controller.WaitForProcessingInput)
        {
            return;
        }

        isProcessingAI = true;

        bool goLeft = controller.GetAction(PlayerController.ACTIONS.LEFT);
        bool goRight = controller.GetAction(PlayerController.ACTIONS.RIGHT);
        bool goUp = controller.GetAction(PlayerController.ACTIONS.UP);
        bool goDown = controller.GetAction(PlayerController.ACTIONS.DOWN);
        bool doJump = controller.GetAction(PlayerController.ACTIONS.JUMP);
        bool doAttack1 = controller.GetAction(PlayerController.ACTIONS.ATTACK_1);
        bool doAttack2 = controller.GetAction(PlayerController.ACTIONS.ATTACK_2);

        // Do real AI
        ProcessAI(ref goLeft, ref goRight, ref goUp, ref goDown, ref doJump, ref doAttack1, ref doAttack2);

        if (tsEnabled)
        {
            controller.SetAction(PlayerController.ACTIONS.LEFT, goLeft);
            controller.SetAction(PlayerController.ACTIONS.RIGHT, goRight);
            controller.SetAction(PlayerController.ACTIONS.UP, goUp);
            controller.SetAction(PlayerController.ACTIONS.DOWN, goDown);
            controller.SetAction(PlayerController.ACTIONS.JUMP, doJump);
            controller.SetAction(PlayerController.ACTIONS.ATTACK_1, doAttack1);
            controller.SetAction(PlayerController.ACTIONS.ATTACK_2, doAttack2);
        }
        controller.WaitForProcessingInput = true;

        isProcessingAI = false;
    }

    private void ProcessAI(ref bool goLeft, ref bool goRight, ref bool goUp, ref bool goDown,
        ref bool doJump, ref bool doAttack1, ref bool doAttack2)
    {
        goLeft = true;
        goRight = false;
        goUp = false;
        goDown = false;
        doJump = !doJump;
        doAttack1 = !doAttack1;
        doAttack2 = false;
    }

}
