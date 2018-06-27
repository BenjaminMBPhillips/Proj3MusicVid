using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public static CameraScript instance;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);

        }
        else
        {
            instance = this;
        }
    }

    public float smoothdamp = 0.5f;

    public Vector3 offset;
    private Vector3 velocity;


    public bool inRaceCamMode = true;
    public Transform minX;
    public Transform maxX;

    public Transform minY;
    public Transform maxY;
    public List<Transform> targets;

    public float minZoom;
    public float maxZoom;
    GameObject[] allPlayers;
    public float zoomLimiter = 50;
    private Camera cam;
    private void Start()
    {
        cam = gameObject.GetComponentInChildren<Camera>();

    }

    public void Add(Transform targToAdd)
    {
        if (!targets.Contains(targToAdd.transform))
            targets.Add(targToAdd.transform);
    }

    public void Remove(Transform targToRemove)
    {
        if (targets.Contains(targToRemove.transform))
        {
            targets.Remove(targToRemove.transform);
        }
    }

    private void FixedUpdate()
    {
        if (inRaceCamMode)
        {
            //transform.position = new Vector2(
            ////Mathf.Clamp(transform.position.x, minX.position.x, maxX.position.x),
            ////Mathf.Clamp(transform.position.y, minY.position.y, maxY.position.y));



            if (targets.Count == 0)
                return;

            Move();
            Zoom();
        }

    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
    void Move()
    {
        Vector3 centorPoint = GetCentorPoint();

        Vector3 newPosition = centorPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothdamp);
    }

    Vector3 GetCentorPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }


}
