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

    // Start is called before the first frame update
    void Start()
    {
        closestText.text = "";
        pickups = GameObject.FindGameObjectsWithTag("PickUp");
        lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClosest();
    }

    float DistanceFromPlayer(GameObject obj) {
        Vector3 playerpos = player.transform.position;
        Vector3 difference = obj.transform.position - playerpos;
        float distance = difference.sqrMagnitude;
        return distance;
    }

    void UpdateClosest() 
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
}
