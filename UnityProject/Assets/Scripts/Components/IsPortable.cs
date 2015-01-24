using UnityEngine;
using System.Collections;

public class IsPortable : MonoBehaviour
{

    private float elapsed;
    public float MIN_TIME_BETWEEN_JUMPS = 2;
    public GameObject lastPortal;

    private CharacterController2D _controller;

    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsed > 0)
        {
            elapsed += Time.deltaTime;
            if (elapsed > MIN_TIME_BETWEEN_JUMPS)
            {
                elapsed = 0;
                lastPortal = null;
            }
        }
    }

    public bool canTeleport(GameObject portal)
    {
        if ((lastPortal == null) || (lastPortal.GetInstanceID() != portal.GetInstanceID()))
        {
            return true;
        }
        return false;
    }

    public void teleport(Vector2 newCoordinates)
    {
        _controller.move(newCoordinates);
        elapsed = 0.001f;
    }

    public void exitedPortal(GameObject portal)
    {
        if (lastPortal != null && lastPortal.GetInstanceID() == portal.GetInstanceID())
        {
            //Debug.Log("cleaning lastPortal");
            lastPortal = null;
            elapsed = 0;
        }
    }

}
