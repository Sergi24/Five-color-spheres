using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Builder : NetworkBehaviour
{
    public Color temporalColor;
    public GameObject blueSphere, redSphere;
    public GameObject turnColorImage;
    public Color blueColorImage;
    public Color redColorImage;
    public TMPro.TextMeshProUGUI textGuanyador;
    public GameObject finishedMenu;

    private Color initialColor;
    private Renderer rend;
    private GameObject game;

    private int[,] table = new int[51, 51];    //0 azul; 1 rojo
 //   private GameObject[,] tableSphere;

  //  private bool blueTurn;
    private int offsetSpheres = 25;

    void Start()
    {
        rend = GetComponent<Renderer>();
        initialColor = rend.material.color;
        game = GameObject.Find("Game");
   //     blueTurn = true;
        for (int i = 0; i < table.GetLength(1); i++)
        {
            for (int j = 0; j < table.GetLength(1); j++)
            {
                table[i, j] = -1;
            }
        }
        table[offsetSpheres, offsetSpheres] = 2;
    //    tableSphere = new GameObject[50, 50];
        finishedMenu.SetActive(false);

    //    GiveAuthToPlayer();
    }

    void OnMouseDown()
    {
        //     if (game.GetComponent<Game>().CmdIsPositionPosible(transform.position.x, transform.position.y))
        //     {
        //    gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkIdentity.);
        //     this.CmdAddTable(transform.position.x, transform.position.y);
        //   gameObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToServer);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players) {
            player.GetComponent<Player>().setPositionSphere(transform.position.x, transform.position.y, gameObject.gameObject);
        }
    }


    void OnMouseEnter()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (isLocalPlayer)
            {
                if (game.GetComponent<Game>().isPositionPosible(transform.position.x, transform.position.y))
                {
                    rend.material.color = temporalColor;
                }
            }
        }
    }

    void OnMouseExit()
    {
        rend.material.color = initialColor;
    }
}