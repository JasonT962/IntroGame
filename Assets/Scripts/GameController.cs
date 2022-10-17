using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{   
    public GameObject player;
    public TextMeshProUGUI closestText;
    private GameObject[] pickups;
    private LineRenderer lineRenderer;
    GameObject[] debuginfo;
    private enum Mode
    {
        Normal,
        Distance,
        Vision
    };

    private Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        closestText.text = "";
        pickups = GameObject.FindGameObjectsWithTag("PickUp");
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
        mode = Mode.Normal;
        debuginfo = GameObject.FindGameObjectsWithTag("Debug");
        foreach (GameObject item in debuginfo)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ModeHandler();
    }

    float DistanceFromPlayer(GameObject obj) {
        Vector3 playerpos = player.transform.position;
        Vector3 difference = obj.transform.position - playerpos;
        float distance = difference.sqrMagnitude;
        return distance;
    }

    void DistanceMode() 
    {
        GameObject closest = FindClosestPickUp();
        if (closest != null) {
            foreach (GameObject pickup in pickups)
            {
                if (pickup == closest) {
                    pickup.GetComponent<Renderer>().material.color = Color.blue;
                    closestText.text = "Closest distance to cube: " + DistanceFromPlayer(pickup);

                    lineRenderer.SetPosition (0, player.transform.position);
                    lineRenderer.SetPosition(1, pickup.transform.position);
                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                }
                else {
                    pickup.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    GameObject FindClosestPickUp()
    {
        float distance = Mathf.Infinity;
        GameObject closest = null;
        foreach (GameObject pickup in pickups)
        {
            float currentdistance = DistanceFromPlayer(pickup);
            if (currentdistance < distance && pickup.activeSelf)
            {
                closest = pickup;
                distance = currentdistance;
            } 
        }
        return closest;
    }

    void ModeHandler()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mode == Mode.Normal)
            {
                mode = Mode.Distance;
                foreach (GameObject item in debuginfo)
                {
                    item.SetActive(true);
                };
                lineRenderer.enabled = true;
            }
            else if (mode == Mode.Distance)
            {
                mode = Mode.Vision;
                foreach (GameObject pickup in pickups)
                {
                    pickup.GetComponent<Renderer>().material.color = Color.white;
                }
            }
            else
            {
                mode = Mode.Normal;
                lineRenderer.enabled = false;
                foreach (GameObject item in debuginfo)
                {
                    item.SetActive(false);
                };
            }
        }

        switch (mode)
        {
            case Mode.Normal:
                break;
            case Mode.Distance:
                DistanceMode();
                break;
            case Mode.Vision:
                VisionMode();
                break;
        }
    }

    void VisionMode()
    {
        Vector3 playervel = GameObject.Find("Player").GetComponent<Rigidbody>().velocity;
        Vector3 playerdirection = playervel.normalized;
        float closestangle = 0;
        GameObject currentclosest = null;
        foreach (GameObject pickup in pickups)
        {
            if (pickup.activeSelf)
            {
                pickup.GetComponent<Renderer>().material.color = Color.white;
                Vector3 pickupdirection = (pickup.transform.position - player.transform.position).normalized;
                if (pickup == null)
                {
                    print("Yes");
                }
                if (Vector3.Dot(playerdirection, pickupdirection) > closestangle)
                {
                    closestangle = Vector3.Dot(playerdirection, pickupdirection);
                    currentclosest = pickup;
                }
            }
        }
        if (currentclosest != null)
        {
            currentclosest.GetComponent<Renderer>().material.color = Color.green;
            currentclosest.transform.LookAt(player.transform.position);
        }
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, player.transform.position + playervel);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }
}
