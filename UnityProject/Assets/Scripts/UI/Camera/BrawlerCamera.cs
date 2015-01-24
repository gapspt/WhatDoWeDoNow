using UnityEngine;
using System.Collections;
using System;

public class BrawlerCamera : MonoBehaviour
{
    public string targetTag = "Player";
    public float minZoom = 1.3f;
    public float smoothDampTime = 0.2f;
    [HideInInspector]
    public new Transform transform;
    public Vector3 cameraOffset;
    public bool useFixedUpdate = false;

    private CharacterController2D _playerController;
    private Vector3 _smoothDampVelocity;
    private float _zoomSmoothVelocity;

    private GameObject[] targetArray;
    public Vector3 targetPosition;
    private Vector3 lastPosition = new Vector3();
    private Vector3 velocity;

    void Awake()
    {
        targetArray = GameObject.FindGameObjectsWithTag(targetTag);

        transform = gameObject.transform;
        //_playerController = target.GetComponent<CharacterController2D>();
    }


    private Vector3 calculateTargetPosition()
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        for (int i = 0; i < targetArray.Length; i++)
        {
            minX = Math.Min(minX, targetArray[i].transform.position.x);
            minY = Math.Min(minY, targetArray[i].transform.position.y);

            maxX = Math.Max(maxX, targetArray[i].transform.position.x);
            maxY = Math.Max(maxY, targetArray[i].transform.position.y);
        }

        //the formulas after you found the min and max values:
        float width = (maxX - minX);
        float height = (maxY - minY);

        float zoom = Math.Max(minZoom, Math.Max(width, height));
        //*1.2 to zoom out a bit farther then needed, the objects would be on the very edge if not

        return new Vector3(minX + width * 0.5f, minY + height * 0.5f, zoom);
    }

    void LateUpdate()
    {
        if (!useFixedUpdate)
            updateCameraPosition();
    }


    void FixedUpdate()
    {
        if (useFixedUpdate)
            updateCameraPosition();
    }


    void updateCameraPosition()
    {
        targetPosition = calculateTargetPosition();
        velocity = (targetPosition - lastPosition) / Time.deltaTime;



        if (velocity.x > 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, targetPosition.z, ref _zoomSmoothVelocity, smoothDampTime);
        }
        else
        {
            var leftOffset = cameraOffset;
            leftOffset.x *= -1;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition - leftOffset, ref _smoothDampVelocity, smoothDampTime);
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, targetPosition.z, ref _zoomSmoothVelocity, smoothDampTime);
        }


        lastPosition = targetPosition;
    }

}