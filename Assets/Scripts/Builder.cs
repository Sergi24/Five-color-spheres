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
    private GameObject game;

    void Start()
    {
        rend = GetComponent<Renderer>();
        initialColor = rend.material.color;
        game = GameObject.Find("Game");
    }

    void OnMouseDown()
    {
        if (game.GetComponent<Game>().isPositionPosible(transform.position.x, transform.position.y))
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
        if (game.GetComponent<Game>().isPositionPosible(transform.position.x, transform.position.y))
        {
            rend.material.color = temporalColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = initialColor;
    }
}