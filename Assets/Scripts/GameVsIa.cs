using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameVsIa : MonoBehaviour
{
    public GameObject blueSphere, redSphere, blackSphere;
    public GameObject turnColorImage;
    public GameObject textYourTurn;

    public Color blueColorImage;
    public Color redColorImage;
    public TMPro.TextMeshProUGUI textGuanyador;
    public GameObject finishedMenu;

    public int mapa;

    private int[,] table;    //0 azul; 1 rojo

    private GameObject[,] tableSphere;
    private GameObject lastSphere;
    private bool blueTurn;
    private bool primeraTiradaOrdinador;
    private int offsetSpheres;

    private int NIVELL_MAX_ALFABETA = 1;

   // private int quadreRang

    private void Start()
    {
        blueTurn = true;
        primeraTiradaOrdinador = true;
        Time.timeScale = 1f;

        mapa = gameObject.GetComponent<MapManager>().getMapa();

        //MAPA0
        table = new int[51, 51];
        offsetSpheres = 25;
        for (int i = 0; i < table.GetLength(1); i++)
        {
            for (int j = 0; j < table.GetLength(1); j++)
            {
                table[i, j] = -1;
                if (mapa == 1 && ((i < 13 || i > 37) || (j < 13 || j > 37))) destroySlot(i, j);
            }
        }
        table[offsetSpheres, offsetSpheres] = 2;
        tableSphere = new GameObject[51, 51];
        
        //MAPA1
        if (mapa == 1)
        {
            table = new int[27, 27];
            offsetSpheres = 13;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if ((i - 1) % 4 == 0 && (j - 1) % 4 == 0)
                    {
                        table[i, j] = 2;
                        destroySlot(i, j);
                        Instantiate(blackSphere, new Vector3(i - offsetSpheres, j - offsetSpheres, 0), transform.rotation);
                    } else if (i==0||j==0||i==table.GetLength(1)-1 || j == table.GetLength(1) - 1) table[i, j] = 2;
                    else table[i, j] = -1;
                }
            }
            tableSphere = new GameObject[27, 27];
        }
    }

    public void addTable(float x, float y)
    {
        GameObject sphereToInstantiate;

        if (blueTurn) table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 0;
        else table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 1;

        if (blueTurn) sphereToInstantiate = blueSphere;
        else sphereToInstantiate = redSphere;
        tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = Instantiate(sphereToInstantiate, new Vector3(x, y, 0), transform.rotation);

        changeColorLastSphere(x, y);

        if (hasWon(Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres, table, true))
        {
            if (blueTurn) textGuanyador.SetText("PLAYER WINS");
            else textGuanyador.SetText("IA WINS");
            textGuanyador.gameObject.SetActive(true);
            Time.timeScale = 0f;
            finishedMenu.SetActive(true);
        }
        else changeTurn();
    }

    private void changeColorLastSphere(float x, float y)
    {
        float colorChange = 0.2f;
        if (lastSphere != null)
        {
            if (blueTurn) lastSphere.GetComponent<Renderer>().material.color -= new Color(0f, colorChange, colorChange);
            else lastSphere.GetComponent<Renderer>().material.color -= new Color(colorChange, colorChange, 0f);
        }
        lastSphere = tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres];

        if (blueTurn) lastSphere.GetComponent<Renderer>().material.color += new Color(colorChange, colorChange, 0f);
        else lastSphere.GetComponent<Renderer>().material.color += new Color(0f, colorChange, colorChange);
    }

    private bool hasWon(int x, int y, int[,] table, bool paintWinners)
    {
        if (lookLine(x, y, 0, 1, table, paintWinners)) return true;
        else if (lookLine(x, y, 1, 1, table, paintWinners)) return true;
        else if (lookLine(x, y, 1, 0, table, paintWinners)) return true;
        else if (lookLine(x, y, -1, 1, table, paintWinners)) return true;
        else return false;
    }

    private bool lookLine(int x, int y, int incX, int incY, int[,] table, bool paintWinners)
    {
        bool win = false;
        int sameColor;
        int contador = 0;

        //trobar inici fila i/o columna
        int i = x, j = y;
        while (i > 0 && i < table.GetLength(1) - 1 && j > 0 && j < table.GetLength(1) - 1)
        {
            i -= incX;
            j -= incY;
        }

        if (blueTurn) sameColor = 0;
        else sameColor = 1;
        while (i < table.GetLength(1) && j < table.GetLength(1) && i >= 0 && j >= 0 && !win)
        {
            //       Debug.Log("TABLE["+(i - offsetSpheres) + "]["+ (j - offsetSpheres) + "]: "+table[i, j]);
            if (table[i, j] == sameColor) contador++;
            else contador = 0;
            if (contador == 5)
            {
                win = true;
                if (paintWinners) paintSpheresWinned(i, j, incX, incY);
            }
            i += incX;
            j += incY;
        }
        return win;
    }

    private void paintSpheresWinned(int i, int j, int incX, int incY)
    {
        for (int k = 0; k < 5; k++)
        {
        //    Debug.Log("TABLE[" + (i - offsetSpheres) + "][" + (j - offsetSpheres) + "]: " + table[i, j]);
            tableSphere[i, j].GetComponent<Renderer>().material.color = Color.yellow;
            i -= incX;
            j -= incY;
        }
    }

    private void changeTurn()
    {
        if (blueTurn)
        {
            blueTurn = false;
            turnColorImage.GetComponent<Image>().color = redColorImage;
            textYourTurn.SetActive(false);

            int tempsAfegit = 0;
            if (primeraTiradaOrdinador) tempsAfegit = 2;
            if (mapa == 0) Invoke("jugadaOrdinador", 0.3f + tempsAfegit);
            else if (mapa == 1) Invoke("jugadaOrdinador", 0.05f + tempsAfegit);
        }
        else
        {
            blueTurn = true;
            turnColorImage.GetComponent<Image>().color = blueColorImage;
            textYourTurn.SetActive(true);
        }
    }

    public bool isPositionPosible(int[,] table, float x, float y)
    {

        int posX = Convert.ToInt32(x) + offsetSpheres;
        int posY = Convert.ToInt32(y) + offsetSpheres;
        bool isPosible = false;

        //!(x == 0 && y == 0) && 
        if (table[posX, posY]==-1)
        {
            //  Debug.Log("x: " + posX + "y: " + posY);
            if (table[posX + 1, posY] != -1) isPosible = true;
            else if (table[posX - 1, posY] != -1) isPosible = true;
            else if (table[posX, posY + 1] != -1) isPosible = true;
            else if (table[posX, posY - 1] != -1) isPosible = true;
            else if (table[posX + 1, posY + 1] != -1) isPosible = true;
            else if (table[posX - 1, posY + 1] != -1) isPosible = true;
            else if (table[posX + 1, posY - 1] != -1) isPosible = true;
            else if (table[posX - 1, posY - 1] != -1) isPosible = true;
        }

        return isPosible;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool getBlueTurn()
    {
        return blueTurn;
    }

    public int[,] getTable()
    {
        return table;
    }

    //IA
    public void jugadaOrdinador()
    {
        Node node = alfaBeta(new Node(table, -1, -1), 0, -2, -1);
        addTable(node.getUltimaTiradaX()-offsetSpheres, node.getUltimaTiradaY()-offsetSpheres);

        destroySlot(node.getUltimaTiradaX(), node.getUltimaTiradaY());
        if (primeraTiradaOrdinador) primeraTiradaOrdinador = false;
    }

    private void destroySlot(int x, int y)
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot");
        int i = 0;
        bool trobat = false;
        while (i < slots.Length && !trobat)
        {
            if (Convert.ToInt32(slots[i].transform.position.x) == x - offsetSpheres && Convert.ToInt32(slots[i].transform.position.y) == y - offsetSpheres)
            {
                trobat = true;
                Destroy(slots[i]);
            }
            i++;
        }
    }

    public Node alfaBeta(Node node, int nivell, int alfa, int beta)
    {
        Node nodeResultat, nodeARetornar;

        if (hasWon(node.getUltimaTiradaX(), node.getUltimaTiradaY(), node.getTable(), false))
        {
        //    Debug.Log("HAS WON");
            if (nivell % 2 == 0) node.setValorHeuristica(-2);  //-2 el tractem com un +infinit
            else node.setValorHeuristica(-1);               //-1 el tractem com un -infinit
            return node;
        }
        else if (nivell == NIVELL_MAX_ALFABETA)
        {
        //    Debug.Log("nivell==NIVELL_MAX_ALFABETA "+nivell);
            node.setValorHeuristica(calcularHeuristica(node, nivell));
            return node;
        }
        else
        {
      //      Debug.Log("nivell: " + nivell);
            nodeARetornar = new Node(node.getTable(), -1, -1);
            if (nivell % 2 == 0) nodeARetornar.setValorHeuristica(-2);          //MIN
            else nodeARetornar.setValorHeuristica(-1);                          //MAX

            ArrayList subNodes = calcularSubNodes(node, nivell);
            int i = 0;
            while ((i < subNodes.Count) && (alfa < beta || alfa == -2 || beta == -1))
            {
                Node subNode = (Node)subNodes[i];
                nodeResultat = alfaBeta(subNode, nivell + 1, alfa, beta);

                if (nivell % 2 == 0)
                {
                    if (nodeResultat.getValorHeuristica() == -1)
                    {
                        nodeARetornar = subNode;
                        nodeARetornar.setValorHeuristica(-1);
                        break;
                    }
                    if (nodeResultat.getValorHeuristica() > alfa || nodeARetornar.getValorHeuristica() == -2)
                    {
                        alfa = nodeResultat.getValorHeuristica();
                        nodeARetornar = subNode;
                        nodeARetornar.setValorHeuristica(alfa);
                    }
                }
                else
                {
                    if (nodeResultat.getValorHeuristica() == -2)
                    {
                        nodeARetornar = subNode;
                        nodeARetornar.setValorHeuristica(-2);
                        break;
                    }
                    if (nodeResultat.getValorHeuristica() < beta || nodeARetornar.getValorHeuristica() == -1)
                    {
                        beta = nodeResultat.getValorHeuristica();
                        nodeARetornar = subNode;
                        nodeARetornar.setValorHeuristica(beta);
                    }
                }
                i++;
            }
            //if (nivell == 0 && nodeARetornar.getValorHeuristica() == -1) Debug.Log("----GUANYARA SI O SI----");
            return nodeARetornar;
        }
    }

    public ArrayList calcularSubNodes(Node node, int nivell)
    {
        ArrayList nodesResultat = new ArrayList();

        for (int i = 1; i < table.GetLength(1) - 1; i++)
        {
            for (int j = 1; j < table.GetLength(1) - 1; j++)
            {
               // Debug.Log("table["+i+"]["+j+"]: "+node.getTable()[i, j]);
                if (isPositionPosible(node.getTable(), i-offsetSpheres, j-offsetSpheres)){
                //    Debug.Log("CREAT SUBNODE i: " + i + " j: " + j);
                    int[,] newTable = node.getTable();
                    if (nivell % 2 == 0) newTable[i, j] = 1;
                    else newTable[i, j] = 0;
                    nodesResultat.Add(new Node(newTable, i, j));
                }
            }
        }

        return nodesResultat;
    }

    public int calcularHeuristica(Node node, int nivell)
    {
        int sumaHeuristica;

        if (!primeraTiradaOrdinador)
        {
            sumaHeuristica = playerCount(node);

            //   Debug.Log("-----------------ULTIMA TIRADA X: " + (node.getUltimaTiradaX()) + " ULTIMA TIRADA Y: " + (node.getUltimaTiradaY()) + "-----------------");
            sumaHeuristica += countOrientation(node, node.getUltimaTiradaX(), node.getUltimaTiradaY(), 0, 1, nivell);
            sumaHeuristica += countOrientation(node, node.getUltimaTiradaX(), node.getUltimaTiradaY(), 1, 1, nivell);
            sumaHeuristica += countOrientation(node, node.getUltimaTiradaX(), node.getUltimaTiradaY(), 1, 0, nivell);
            sumaHeuristica += countOrientation(node, node.getUltimaTiradaX(), node.getUltimaTiradaY(), -1, 1, nivell);
        }
        else
        {
            sumaHeuristica = lookWhereIsPlayer(node.getTable(), node.getUltimaTiradaX(), node.getUltimaTiradaY());
        }
       // Debug.Log("HEURISTICA(" + (node.getUltimaTiradaX() - offsetSpheres) + ")(" + (node.getUltimaTiradaY() - offsetSpheres) + "): " + sumaHeuristica);
        return 100000 + sumaHeuristica;
    }

    private int lookWhereIsPlayer(int[,] table, int x, int y)
    {
        int posX = x;
        int posY = y;
        int suma = 0;

        //!(x == 0 && y == 0) && 
        //  Debug.Log("x: " + posX + "y: " + posY);
        if (table[posX + 1, posY] == 0) suma = 1;
        else if (table[posX - 1, posY] == 0) suma = 1;
        else if (table[posX, posY + 1] == 0) suma = 1;
        else if (table[posX, posY - 1] == 0) suma = 1;
        else if (table[posX + 1, posY + 1] == 0) suma = 1;
        else if (table[posX - 1, posY + 1] == 0) suma = 1;
        else if (table[posX + 1, posY - 1] == 0) suma = 1;
        else if (table[posX - 1, posY - 1] == 0) suma = 1;

        return suma;
    }

    private int countOrientation(Node node, int x, int y, int incX, int incY, int nivell)
    {
        bool final = false;
        int sameColor;
        int contador = 0;
        int suma = 0, bloqueig = 0;

        int i = x, j = y;
        int posInicialX = x, posInicialY = y;

    //    Debug.Log("////////////////INC X: " + incX + " INC Y: " + incY + "////////////////");

        if (nivell % 2 == 0) sameColor = 0;
        else sameColor = 1;
        while (i < table.GetLength(1) && j < table.GetLength(1) && i >= 0 && j >= 0 && !final)
        {
            if (node.getTable()[i, j] == sameColor) contador++;
            else
            {
                if (node.getTable()[i, j] != -1) bloqueig += 1;
                final = true;
            }

            i += incX;
            j += incY;
        }
        final = false;
        i = posInicialX-incX;
        j = posInicialY-incY;
        while (i < table.GetLength(1) && j < table.GetLength(1) && i >= 0 && j >= 0 && !final)
        {
            if (node.getTable()[i, j] == sameColor) contador++;
            else
            {
                if (node.getTable()[i, j] != -1) bloqueig += 1;
                final = true;
            }

            i -= incX;
            j -= incY;
        }

    //    Debug.Log("CONTADOR: " + contador + " BLOQUEIG: " + bloqueig);

        if (contador == 4 && bloqueig == 0) suma = 750;
        else if (contador == 4 && bloqueig == 1) suma = 500;
        else if (contador == 3 && bloqueig == 0) suma = 10;
        else if (contador == 3 && bloqueig == 1) suma = 5;
        else if (contador == 2 && bloqueig == 0) suma = 3;
        else if (contador == 2 && bloqueig == 1) suma = 1;
        else if (contador == 1 && bloqueig == 1) suma = -1;

        return suma;
    }

    private int playerCount(Node node)
    {
        int suma = 0;
        for (int i = 0; i < table.GetLength(1); i++) {
            suma -= lookLineCountPlayer(0, i, 1, 0, node.getTable());

            suma -= lookLineCountPlayer(i, 0, 0, 1, node.getTable());
        }
        //DIAGONALS
        //DALT DRETA
        for (int i = 0; i < table.GetLength(1); i++)
        {
            suma -= lookLineCountPlayer(0, i, 1, 1, node.getTable());
        }
        for (int i = 1; i < table.GetLength(1); i++)
        {
            suma -= lookLineCountPlayer(i, 0, 1, 1, node.getTable());
        }
        //DALT ESQUERRA
        /*  for (int i = 0; i < table.GetLength(1); i++)
          {
              suma -= lookLineCountPlayer(i, 0, -1, 1, node.getTable());
          }
             for (int i = table.GetLength(1)-1; i > 0; i--)
             {
                 suma -= lookLineCountPlayer(i, table.GetLength(1)-1, -1, 1, node.getTable());
             }*/
        for (int i = 0; i < table.GetLength(1); i++)
        {
            suma -= lookLineCountPlayer(i, i, -1, 1, node.getTable());
        }
        for (int i = 0; i < table.GetLength(1)-1; i++)
        {
            suma -= lookLineCountPlayer(i+1, i, -1, 1, node.getTable());
        }


        return suma;
    }

    private int lookLineCountPlayer(int x, int y, int incX, int incY, int[,] table)
    {
        int suma = 0, bloqueig = 0;
        int sameColor;
        int contador = 0, contadorEspais = 0;

        //trobar inici fila i/o columna
        int i = x, j = y;
        while (i > 0 && i < table.GetLength(1)-1 && j > 0 && j < table.GetLength(1)-1)
        {
            i -= incX;
            j -= incY;
        }

        sameColor = 0;
        while (i < table.GetLength(1) && j < table.GetLength(1) && i >= 0 && j >= 0)
        {
            if (table[i, j] == sameColor) {
                contador++;
            //    Debug.Log("CONTADOR++ EN i: " + i + " j: " + j );
            }
    //        else if (table[i, j] == -1) contadorEspais++;
            else if (contador != 0)
            {
                bool final = false, pont = false;
                contadorEspais = contador+1;
                int contadorInicial = contador;

                //bloqueig davant
                if (table[i, j] != -1) bloqueig += 1;
                else //jugades intermitjes    [i, j]==-1
                {
                    while (!final && contadorEspais < 5)
                    {
                        int nextPosition = table[i + ((contadorEspais - contadorInicial) * incX), j + ((contadorEspais - contadorInicial) * incY)];
                        if (nextPosition == sameColor) //nova esfera
                        {
                            contador++;
                            pont = true;
                      //      Debug.Log("AQUI. contador++");
                        }
                        else if (nextPosition != -1 && pont) //bloqueig
                        {
                            final = true;
                            bloqueig += 1;
                        }
                        else final = true; //segon espai en blanc
                        contadorEspais++;
                    }
                }

                //bloqueig darrere
            //    Debug.Log("TABLE[" + (i-incX) + "][" + (j-incY) + "]: " + table[i-incX, j-incY]);
                if (table[i - (incX * (contadorInicial + 1)), j - (incY * (contadorInicial + 1))] != -1) bloqueig += 1;

                if (contador >= 4 && bloqueig == 0) suma += 1000;
                else if (contador >= 4 && bloqueig == 1) suma += 1000;
                else if (contador == 3 && bloqueig == 0) suma += 500;
                else if (contador == 3 && bloqueig == 1) suma += 5;
                else if (contador == 2 && bloqueig == 0) suma += 2;
                else if (contador == 2 && bloqueig == 1) suma += 1;
                contador = 0;
                contadorEspais = 0;
                bloqueig = 0;
            }
            
            i += incX;
            j += incY;
        }
        return suma;
    }

}
