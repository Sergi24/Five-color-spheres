using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    private GameObject game;
    private float posX;
    private float posY;
    private bool sphereHasBeenCreated;
    private GameObject sphereChoosed;
    private int numPlayer;


    private void Start()
    {
        game = GameObject.Find("Game");
        sphereHasBeenCreated = false;
        if (isLocalPlayer)
        {
            //CmdIncrementarNumPlayers();
            numPlayer = GameObject.FindGameObjectsWithTag("Player").Length;
            Debug.Log("NumJugador: " + numPlayer);
        }
        if (!isLocalPlayer) gameObject.GetComponentInChildren<AudioListener>().enabled=false;
    }

    public void setPositionSphere(float x, float y, GameObject sphere)
    {
        posX = x;
        posY = y;
        sphereHasBeenCreated = true;
        sphereChoosed = sphere;
    }

    private void Update()
    {
        if (sphereHasBeenCreated)
        {
            if (isLocalPlayer)
            {
                if (game.GetComponent<Game>().isMyTurn(numPlayer) && GameObject.FindGameObjectsWithTag("Player").Length>1)
                {
                    CmdAddTable(posX, posY, sphereChoosed);
                }
            }
            sphereHasBeenCreated = false;
        } 
    }

    [Command]
    private void CmdAddTable(float posX, float posY, GameObject sphere)
    {
        game.GetComponent<Game>().addTable(posX, posY, sphere);
    }
}
