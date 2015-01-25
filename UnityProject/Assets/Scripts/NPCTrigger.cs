using UnityEngine;
using System.Collections;

public class NPCTrigger : MonoBehaviour {
    // Actions to do
    public bool Jump = true;
    public bool Reverse = false;

    // From where
    public bool FromLeft = true;
    public bool FromRight = true;

    void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            foreach (var c in GetComponents<Renderer>())
            {
                Destroy(c);
            }
        }
    }
}
