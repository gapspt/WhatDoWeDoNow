using UnityEngine;
using System.Collections;

public class PortalBehaviour : MonoBehaviour
{

    public GameObject connectedPortal;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Portal " + name + " Object was triggered by" + other.gameObject.name);

        IsPortable obj = other.gameObject.GetComponent<IsPortable>();
        if (obj && obj.canTeleport(gameObject))
        {
            obj.lastPortal = connectedPortal;
            obj.teleport(new Vector2(connectedPortal.transform.position.x, connectedPortal.transform.position.y));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IsPortable obj = other.gameObject.GetComponent<IsPortable>();
        if (obj)
        {
            obj.exitedPortal(gameObject);
        }
    }
}
