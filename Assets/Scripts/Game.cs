using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Game : NetworkBehaviour
{
    public GameObject blueSphere, redSphere;
    public GameObject turnColorImage;
    public GameObject textYourTurn;
    public GameObject finishedRestart;
    public Color blueColorImage;
    public Color redColorImage;
    public TMPro.TextMeshProUGUI textGuanyador;
    public GameObject finishedMenu;

    private int[,] table = new int[51, 51];    //0 azul; 1 rojo
    private int numPlayer;
    private GameObject[,] tableSphere;
    [SyncVar]
    private bool blueTurn;
    private int offsetSpheres = 25;
    private GameObject lastSphere;

    private void Start()
    {
        if (isServer) blueTurn = true;

        if (blueTurn) turnColorImage.GetComponent<Image>().color = blueColorImage;
        else turnColorImage.GetComponent<Image>().color = redColorImage;
        for (int i=0; i<table.GetLength(1); i++)
        {
            for (int j = 0; j <table.GetLength(1); j++)
            {
                table[i, j] = -1;
            }
        }
        table[offsetSpheres, offsetSpheres] = 2;
        tableSphere = new GameObject[50, 50];
        finishedMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void setNumPlayer(int numPlayer)
    {
        this.numPlayer = numPlayer;
        if (numPlayer == 1) textYourTurn.SetActive(true);
    }

    public void addTable(float x, float y, GameObject sphere, int numPlayer)
    {
        GameObject sphereToInstantiate;

        Destroy(sphere);

        if (blueTurn) table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 0;
        else table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 1;

        if (blueTurn) sphereToInstantiate = blueSphere;
        else sphereToInstantiate = redSphere;
        tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = Instantiate(sphereToInstantiate, new Vector3(x, y, 6.43f), transform.rotation);
        NetworkServer.Spawn(tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres]);

        RpcAddTable(x, y, blueTurn, tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres]);

        RpcChangeColorLastSphere(x, y, numPlayer);

        if (hasWon(Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres))
        {
            RpcPlayerHasWon(blueTurn);
        } else changeTurn(blueTurn);
    }

    [ClientRpc]
    private void RpcChangeColorLastSphere(float x, float y, int numPlayer)
    {
        Debug.Log("numPlayer: "+numPlayer+"|| this.numPlayer: "+this.numPlayer);
        float colorChange = 0.2f;

        if (lastSphere != null)
        {
            if ((this.numPlayer == 1 && numPlayer == 1) || (this.numPlayer == 2 && numPlayer == 2))
            {
                if (blueTurn) lastSphere.GetComponent<Renderer>().material.color -= new Color(0f, colorChange, colorChange);
                else lastSphere.GetComponent<Renderer>().material.color -= new Color(colorChange, colorChange, 0f);
            }
        }
        lastSphere = tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres];
        if ((this.numPlayer == 1 && numPlayer == 2) || (this.numPlayer == 2 && numPlayer == 1))
        {
            if (blueTurn) lastSphere.GetComponent<Renderer>().material.color += new Color(colorChange, colorChange, 0f);
            else lastSphere.GetComponent<Renderer>().material.color += new Color(0f, colorChange, colorChange);
        }
    }

    [ClientRpc]
    private void RpcAddTable(float x, float y, bool isBlueTurn, GameObject sphere)
    {
   //     Debug.Log("ClientRpc: x:" + Convert.ToInt32(x) + ", y:" + Convert.ToInt32(y)+ " BlueTurn: " + isBlueTurn);
        if (isBlueTurn) table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 0;
        else table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 1;

        tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = sphere;
    }

    [ClientRpc]
    private void RpcPlayerHasWon(bool isBlueTurn)
    {
   //     Debug.Log("RPC PLAYER HAS WON:");
        if (isBlueTurn) textGuanyador.SetText("BLUE PLAYER WINS");
        else textGuanyador.SetText("RED PLAYER WINS");
        textGuanyador.gameObject.SetActive(true);
        if (numPlayer!=1) finishedRestart.gameObject.SetActive(false);
        Time.timeScale = 0f;
        finishedMenu.SetActive(true);
    }

    private bool hasWon(int x, int y)
    {
        if (lookLine(x, y, 0, 1)) return true;
        else if (lookLine(x, y, 1, 1)) return true;
        else if (lookLine(x, y, 1, 0)) return true;
        else if (lookLine(x, y, -1, 1)) return true;
        else return false;
    }

    private bool lookLine(int x, int y, int incX, int incY)
    {
        bool win = false;
        int sameColor;
        int contador = 0;
        
        //trobar inici fila i/o columna
        int i = x, j = y;
        while (i > 0 && i < table.GetLength(1)-1 && j > 0 && j < table.GetLength(1)-1)
        {
            i -= incX;
            j -= incY;
        }

        if (blueTurn) sameColor = 0;
        else sameColor = 1;
        while (i < table.GetLength(1) && j < table.GetLength(1) && i >= 0 && j >=0 && !win)
        {
            //       Debug.Log("TABLE["+(i - offsetSpheres) + "]["+ (j - offsetSpheres) + "]: "+table[i, j]);
            if (table[i, j] == sameColor) contador++;
            else contador = 0;
            if (contador == 5)
            {
                win = true;
                RpcPaintSpheresWinned(i, j, incX, incY);
            }
            i += incX;
            j += incY;
        }
        return win;
    }

    [ClientRpc]
    private void RpcPaintSpheresWinned(int i, int j, int incX, int incY)
    {
        for (int k = 0; k < 5; k++)
        {
            tableSphere[i, j].GetComponent<Renderer>().material.color = Color.yellow;
            i -= incX;
            j -= incY;
        }
    }

    private void changeTurn(bool isBlueTurn)
    {
        RpcChangeTurn(isBlueTurn);

        if (isBlueTurn) blueTurn = false;
        else blueTurn = true;
    }

    [ClientRpc]
    private void RpcChangeTurn(bool isBlueTurn)
    {
        if (isBlueTurn)
        {
            turnColorImage.GetComponent<Image>().color = redColorImage;
            if (numPlayer==2) textYourTurn.SetActive(true);
            else textYourTurn.SetActive(false);
        }
        else
        {
            turnColorImage.GetComponent<Image>().color = blueColorImage;
            if (numPlayer == 1) textYourTurn.SetActive(true);
            else textYourTurn.SetActive(false);
        }
    }

    public bool isMyTurn(int numPlayer)
    {
        if (numPlayer == 1 && blueTurn) return true;
        else if (numPlayer == 2 && !blueTurn) return true;
        else return false;
    }

    public bool isPositionPosible(float x, float y)
    {
        int posX = Convert.ToInt32(x) + offsetSpheres;
        int posY = Convert.ToInt32(y) + offsetSpheres;
        bool isPosible = false;

        if (table[posX + 1, posY] != -1 ) isPosible = true;
        else if (table[posX - 1, posY] != -1) isPosible = true;
        else if (table[posX, posY + 1] != -1) isPosible = true;
        else if (table[posX, posY - 1] != -1) isPosible = true;
        else if (table[posX + 1, posY + 1] != -1) isPosible = true;
        else if (table[posX - 1, posY + 1] != -1) isPosible = true;
        else if (table[posX + 1, posY - 1] != -1) isPosible = true;
        else if (table[posX - 1, posY - 1] != -1) isPosible = true;

        return isPosible;
    }


    public void Restart()
    {
        RpcRestart();
    }

    [ClientRpc]
    private void RpcRestart()
    {
        SceneManager.LoadSceneAsync("Joc");
    }
}