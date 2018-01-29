using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    public GameObject mapa1;
    public GameObject mapa2;
    public Color colorMapaEscollit;

    private void Start()
    {
        SetMapa1Button();
    }

    public void OnePlayerMode()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().enabled = false;
        SceneManager.LoadScene("JocVsIa");
    }

    public void SetMapa1Button()
    {
        mapa1.GetComponent<Image>().color = colorMapaEscollit;
        mapa2.GetComponent<Image>().color = Color.white;
    }

    public void SetMapa2Button()
    {
        mapa1.GetComponent<Image>().color = Color.white;
        mapa2.GetComponent<Image>().color = colorMapaEscollit;
    }
}
