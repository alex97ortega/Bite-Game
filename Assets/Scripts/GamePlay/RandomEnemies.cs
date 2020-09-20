using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemies : MonoBehaviour
{
    public Personaje[] personajes;
    public GestorPartida gestor;
    public Terreno tablero;
    public Material rojoEnemigo;

    // Start is called before the first frame update
    void Awake()
    {
        gestor.Init();
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            List<int> enemies = new List<int>();

            for(int i = 0; i < gm.GetNumJugadoresCombate();i++)
            {
                int x;
                do
                {
                    x = Random.Range(0, personajes.Length);
                } while (enemies.Contains(x));
                enemies.Add(x);
            }

            int n = 0;
            foreach (int i in enemies)
            {
                gestor.SetEnemigo(Instantiate(personajes[i]), n);
                int z;
                do
                {
                    z = Random.Range(0, tablero.GetColumnas());
                } while (tablero.GetCasilla(0, z).EstaOcupada());
                tablero.GetCasilla(0, z).Ocupar();

                gestor.GetEnemigo(n).SetPos(0, z);
                gestor.GetEnemigo(n).Girar();
                gestor.GetEnemigo(n).SetColor(rojoEnemigo);
                n++;
            }
        }
    }
}
