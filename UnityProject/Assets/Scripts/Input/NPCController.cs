using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterController2D characterController;

    private bool isProcessingAI = false;

    public bool IsGoingLeft;
    public bool DoJump = false;

    // Thread safe variables (because the original variables cannot be accessed outside of the main thread)
    private bool tsEnabled;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController2D>();
        IsGoingLeft = Random.Range(0, 2) == 0;
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
        if (playerController.WaitForProcessingInput)
        {
            return;
        }

        isProcessingAI = true;

        bool goLeft = playerController.GetAction(PlayerController.ACTIONS.LEFT);
        bool goRight = playerController.GetAction(PlayerController.ACTIONS.RIGHT);
        bool goUp = playerController.GetAction(PlayerController.ACTIONS.UP);
        bool goDown = playerController.GetAction(PlayerController.ACTIONS.DOWN);
        bool doJump = playerController.GetAction(PlayerController.ACTIONS.JUMP);
        bool doAttack1 = playerController.GetAction(PlayerController.ACTIONS.ATTACK_1);
        bool doAttack2 = playerController.GetAction(PlayerController.ACTIONS.ATTACK_2);

        // Do real AI
        ProcessAI(ref goLeft, ref goRight, ref goUp, ref goDown, ref doJump, ref doAttack1, ref doAttack2);

        if (tsEnabled)
        {
            playerController.SetAction(PlayerController.ACTIONS.LEFT, goLeft);
            playerController.SetAction(PlayerController.ACTIONS.RIGHT, goRight);
            playerController.SetAction(PlayerController.ACTIONS.UP, goUp);
            playerController.SetAction(PlayerController.ACTIONS.DOWN, goDown);
            playerController.SetAction(PlayerController.ACTIONS.JUMP, doJump);
            playerController.SetAction(PlayerController.ACTIONS.ATTACK_1, doAttack1);
            playerController.SetAction(PlayerController.ACTIONS.ATTACK_2, doAttack2);
        }
        playerController.WaitForProcessingInput = true;

        isProcessingAI = false;
    }

    private void ProcessAI(ref bool goLeft, ref bool goRight, ref bool goUp, ref bool goDown,
        ref bool doJump, ref bool doAttack1, ref bool doAttack2)
    {
        /*
        if (goLeft && characterController.collisionState.left)
        {
            goRight = true;
            goLeft = false;
        }
        if (goRight && characterController.collisionState.right)
        {
            goLeft = true;
            goRight = false;
        }

        if (goLeft && goRight)
        {
            goLeft = true;
            goRight = false;
        }

        if (!goLeft && !goRight)
        {
            goLeft = true;
            goRight = false;
        }
        */

        goLeft = IsGoingLeft;
        goRight = !IsGoingLeft;
        doJump = DoJump;
        doAttack1 = !doAttack1;

        doAttack2 = false;
        goUp = false;
        goDown = false;

        DoJump = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        foreach (var trigger in other.transform.GetComponentsInChildren<NPCTrigger>())
        {
            if (!DoJump && trigger.ShouldJump(IsGoingLeft))
            {
                DoJump = true;
            }

            if ((!IsGoingLeft && trigger.ShouldGoLeft(IsGoingLeft))
                || (IsGoingLeft && trigger.ShouldGoRight(IsGoingLeft)))
            {
                print("reverse");
                IsGoingLeft = !IsGoingLeft;
            }
        }
    }
}
