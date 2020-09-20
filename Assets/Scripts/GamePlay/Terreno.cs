using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terreno : MonoBehaviour
{
    //public
    public Casilla casilla;
    public int filas, columnas;
    public GestorPartida gestor;
    public Material azulAliado;
    //private
    private Casilla[,] tablero;
    private GameManager gm;

    // Start is called before the first frame update
    void Awake()
    {
        gestor.Init();
        tablero = new Casilla[filas, columnas];

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                tablero[i, j] = Instantiate(casilla);
                tablero[i, j].transform.SetParent(gameObject.transform);
                tablero[i, j].SetPos(i * 3, 0, j * 3);
            }
        }

        // instanciamos los personajes
        gm = FindObjectOfType<GameManager>();
        if(gm)
        {
            int n = 0;
            foreach (Personaje p in gm.GetJugadoresPartida())
            {
                gestor.SetAliado(Instantiate(p),n);
                int z ;
                do
                {
                    z = Random.Range(0, columnas);
                } while (tablero[filas - 1, z].EstaOcupada());
                tablero[filas-1, z].Ocupar();

                gestor.GetAliado(n).SetPos(filas - 1, z);
                gestor.GetAliado(n).SetColor(azulAliado);
                n++;
            }
        }
    }
    public Casilla GetCasilla(int x, int z) { return tablero[x, z]; }
    public int GetFilas() { return filas; }
    public int GetColumnas() { return columnas; }
}
