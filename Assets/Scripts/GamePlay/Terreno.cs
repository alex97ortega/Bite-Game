using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terreno : MonoBehaviour
{
    //public
    public Casilla casilla;
    public int filas, columnas;
    public GestorPartida gestor;
    public Material azulAliado, casillaNormal, casillaAmarilla;
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
                tablero[i, j].SetPos(i, 0, j);
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

                tablero[filas-1, z].Ocupar(gestor.GetAliado(n));

                gestor.GetAliado(n).SetPos(filas - 1, z);
                gestor.GetAliado(n).SetColor(azulAliado);
                n++;
            }
        }
    }
    public Casilla GetCasilla(int x, int z) { return tablero[x, z]; }
    public int GetFilas() { return filas; }
    public int GetColumnas() { return columnas; }

    public void RestauraTablero()
    {
        foreach (var c in tablero)
            c.GetComponent<MeshRenderer>().material = casillaNormal;
    }

    public void PintaCasillasAmarillas(int casX, int casZ, int distancia)
    {
        foreach (var c in tablero)
        {
            int posibleDesplazamiento = (int)Mathf.Abs(casX - c.GetCas().x) + (int)Mathf.Abs(casZ - c.GetCas().z);
            if (posibleDesplazamiento <= distancia)
            {
                if(!c.EstaOcupada())
                    c.GetComponent<MeshRenderer>().material = casillaAmarilla;
            }
        }
    }
    public bool PuedeMoverACasilla(int casX, int casZ)
    {
        if (casX < 0 || casZ < 0)
            return false;
        if (casX >= filas || casZ >= columnas)
            return false;
        return !tablero[casX, casZ].EstaOcupada();
    }
    public void EnvenenarCasillas(int casX, int casZ, int rango, int dmg)
    {
        if(rango == 0)
        {
            tablero[casX, casZ].Envenenar(dmg);
            return;
        }

        for (int i = -rango; i <= rango; i++)
        {
            for (int j = -rango; j <= rango; j++)
            {
                int newCasX = casX + i;
                int newCasZ = casZ + j;
                if (newCasX >= 0 && newCasX < filas && newCasZ >= 0 && newCasZ < columnas)
                {
                    tablero[newCasX, newCasZ].Envenenar(dmg);
                }
            }
        }
    }

    public void GestionaEnvenenamientosCasillas(string nombreP)
    {
        if (nombreP == "Reygon") //inmune
            return;

        foreach (var c in tablero)
        {
            if (c.EstaEnvenenada() && c.EstaOcupada() && (c.GetPersonajeCasilla().nombre == nombreP))
                c.GetPersonajeCasilla().HacerDanyo(c.GetVenenoDmg());
        }
    }

    public Personaje EncuentraAliadoAdyacente(int casX, int casZ, bool aliado)
    {

        if (casX != 0)
        {
            Personaje aux = tablero[casX - 1, casZ].GetPersonajeCasilla();

            if (aux != null && (aux.IsAliado() == aliado))
                return aux;
        }
        if (casX != filas-1)
        {
            Personaje aux = tablero[casX + 1, casZ].GetPersonajeCasilla();

            if (aux != null && (aux.IsAliado() == aliado))
                return aux;
        }
        if (casZ != 0)
        {
            Personaje aux = tablero[casX, casZ - 1].GetPersonajeCasilla();

            if (aux != null && (aux.IsAliado() == aliado))
                return aux;
        }
        if (casZ != columnas-1)
        {
            Personaje aux = tablero[casX, casZ + 1].GetPersonajeCasilla();

            if (aux != null && (aux.IsAliado() == aliado))
                return aux;
        }

        return null;
    }
}
