using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemies : MonoBehaviour
{
    public Personaje[] personajes;
    public GestorPartida gestor;
    public Terreno tablero;
    public Material rojoEnemigo;
    
    public void GenerateRandomEnemies()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            List<int> enemies = new List<int>();

            for (int i = 0; i < gm.GetNumJugadoresCombate(); i++)
            {
                int x;
                do
                {
                    x = Random.Range(0, personajes.Length);
                    foreach(var aly in gestor.GetAllAliados())
                    {
                        if (aly.id == x)
                        {
                            //Debug.Log("Enemigo " + x + ", no es valido");
                            x = -1; // si ya tenemos un aliado de este tipo, no es valido
                        }
                    }
                } while (x == -1 || enemies.Contains(x));

                //Debug.Log("Enemigo " + x + " valido");
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

                tablero.GetCasilla(0, z).Ocupar(gestor.GetEnemigo(n));

                gestor.GetEnemigo(n).SetPos(0, z);
                gestor.GetEnemigo(n).Girar();
                gestor.GetEnemigo(n).SetEnemigo();
                gestor.GetEnemigo(n).SetColor(rojoEnemigo);
                n++;
            }
        }
    }
}
