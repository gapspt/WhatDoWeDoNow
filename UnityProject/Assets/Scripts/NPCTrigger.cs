using UnityEngine;
using System.Collections;

public class NPCTrigger : MonoBehaviour {
    // Actions to do
    public bool Jump = true;
    public bool GoLeft = false;
    public bool GoRight = false;

    // From where
    public bool FromLeft = true;
    public bool FromRight = true;

    public float Probability = 1.0f;


    void Awake()
    {
        if (transform.lossyScale.x < 0)
        {
            Switch(ref FromLeft, ref FromRight);
            Switch(ref GoLeft, ref GoRight);
        }

        if (!Debug.isDebugBuild)
        {
            foreach (var c in GetComponents<Renderer>())
            {
                Destroy(c);
            }
        }
    }
    private void Switch(ref bool a, ref bool b)
    {
        bool temp = a;
        a = b;
        b = temp;
    }

    public bool ShouldJump(bool movingLeft)
    {
        return Jump && ShouldDoAction(movingLeft);
    }

    public bool ShouldGoLeft(bool movingLeft)
    {
        return GoLeft && ShouldDoAction(movingLeft);
    }

    public bool ShouldGoRight(bool movingLeft)
    {
        return GoRight && ShouldDoAction(movingLeft);
    }

    private bool ShouldDoAction(bool movingLeft)
    {
        return ((!movingLeft && FromLeft) || (movingLeft && FromRight)) && Random.Range(0.0f, 1.0f) <= Probability;
    }
}
