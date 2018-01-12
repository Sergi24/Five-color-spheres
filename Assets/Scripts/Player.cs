using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    /*
        public GameObject blueSphere, redSphere;
        public GameObject turnColorImage;
        public Color blueColorImage;
        public Color redColorImage;
        public TMPro.TextMeshProUGUI textGuanyador;
        public GameObject finishedMenu;

        private int[,] table = new int[51, 51];    //0 azul; 1 rojo
        private GameObject[,] tableSphere;
        private bool blueTurn;
        private int offsetSpheres = 25;
    */
    private GameObject game;
    private float posX;
    private float posY;
    private bool sphereHasBeenCreated, sphereIsSelected;
    private GameObject sphereChoosed;
    private int numPlayer;

    /*
        // Use this for initialization
        private void Start()
        {
            blueTurn = true;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    table[i, j] = -1;
                }
            }
            table[offsetSpheres, offsetSpheres] = 2;
            tableSphere = new GameObject[50, 50];
          //  finishedMenu.SetActive(false);
            Time.timeScale = 1f;
        }
*/
    private void Start()
    {
        game = GameObject.Find("Game");
        sphereHasBeenCreated = false;
        sphereIsSelected = false;
        if (isLocalPlayer)
        {
            //       CmdIncrementarNumPlayers();
            numPlayer =// game.GetComponent<Game>().getNumPlayers();
                GameObject.FindGameObjectsWithTag("Player").Length;
            Debug.Log("NumJugador: " + numPlayer);
        }
        if (!isLocalPlayer) gameObject.GetComponentInChildren<AudioListener>().enabled=false;
    }

 /*   [Command]
    private void CmdIncrementarNumPlayers()
    {
        Debug.Log("IncrementarNumPlayers");
        game.GetComponent<Game>().incrementarNumPlayers();
        Debug.Log("numPlayers"+game.GetComponent<Game>().getNumPlayers());
    }*/

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
            Debug.Log("isLocalPlayer:" + isLocalPlayer);
            if (isLocalPlayer)
            {
                if (game.GetComponent<Game>().isMyTurn(numPlayer))
                {
                    CmdAddTable(posX, posY, sphereChoosed);
                }
            }
            sphereHasBeenCreated = false;
        } /*else if (sphereIsSelected)
        {
            if (isLocalPlayer)
            {
                if (game.GetComponent<Game>().isPositionPosible(numPlayer))
                {

                }
            }
        }*/
    }

    [Command]
    private void CmdAddTable(float posX, float posY, GameObject sphere)
    {
        game.GetComponent<Game>().addTable(posX, posY, sphere);
    }

    [Command]
    public void CmdIsPositionPosible(float x, float y)
    {


    }

        /*
            [Command]
            public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player)
            {
                Debug.Log("giving player auth now...");
                var iObject = NetworkServer.FindLocalObject(objectId);
                var networkIdentity = iObject.GetComponent<NetworkIdentity>();
                var otherOwner = networkIdentity.clientAuthorityOwner;

                if (otherOwner == player.connectionToClient)
                {
                    Debug.Log("player already has auth");
                    //return;
                }
                else
                {
                    if (otherOwner != null)
                    {
                        networkIdentity.RemoveClientAuthority(otherOwner);
                    }
                    Debug.Log("player now has auth!");
                    networkIdentity.AssignClientAuthority(player.connectionToClient);
                }

            //    iObject.GetComponent<Builder>().RpcLight();
            }*/
    }
