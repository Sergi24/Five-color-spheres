using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Builder : NetworkBehaviour
{
    public Color temporalColor;

    private Color initialColor;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        initialColor = rend.material.color;
    }

    void OnMouseDown()
    {
        if (GameObject.Find("Game").GetComponent<Game>().isPositionPosible(transform.position.x, transform.position.y))
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                player.GetComponent<Player>().setPositionSphere(transform.position.x, transform.position.y, gameObject.gameObject);
            }
        }
    }


    void OnMouseEnter()
    {
        if (GameObject.Find("Game").GetComponent<Game>().isPositionPosible(transform.position.x, transform.position.y))
        {
            Debug.Log("PossibleTRUE");
            rend.material.color = temporalColor;
        }
        else
        {
            Debug.Log("PossibleFALSE");
        }
    }

    void OnMouseExit()
    {
        rend.material.color = initialColor;
    }
}