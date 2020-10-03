using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorPartida : MonoBehaviour
{
    public GameObject menuExit, menuVictory, menuGameOver;
    public Turno turnoPrefab;
    public GameObject parentTurnos;
    public RandomEnemies randomEnemies;
    public GestorAcciones gestorAcciones;

    Personaje[] aliados;
    Personaje[] enemigos;
    Queue<Turno> turnosJugadores;
    GameManager gm;
    int nJugadoresPorEquipo;

    // Start is called before the first frame update
    void Start()
    {
        randomEnemies.GenerateRandomEnemies();

        Queue<int> turnos = new Queue<int>();
        turnosJugadores = new Queue<Turno>();
        // seleccion aleatoria de turnos
        for (int i = 0; i< nJugadoresPorEquipo * 2; i++)
        {
            int x;
            int veces = 0;
            do
            {
                veces++;
                x = Random.Range(0, nJugadoresPorEquipo * 2);
            } while (turnos.Contains(x) && veces < 1000);
            turnos.Enqueue(x);
        }
        // recorremos entera la cola anterior. Si está Alex, le forzamos a que sea el primero.
        for (int i = 0; i < nJugadoresPorEquipo * 2; i++)
        {
            if (turnos.Peek() < nJugadoresPorEquipo)
            {
                if (aliados[turnos.Peek()].nombre == "Alex")
                    break;
            }
            else 
            {
                if (enemigos[turnos.Peek() - nJugadoresPorEquipo].nombre == "Alex")
                    break;
            }
            int turnoAux = turnos.Dequeue();
            turnos.Enqueue(turnoAux);
        }

        // ahora sí vamos creando los personajes
        int num = 0;
        foreach (int x in turnos)
        {
            Turno aux = Instantiate(turnoPrefab);
            if (x < nJugadoresPorEquipo)
                aux.SetProperties(x, aliados[x].GetFoto());
            else
                aux.SetProperties(x, enemigos[x - nJugadoresPorEquipo].GetFoto());
            aux.transform.SetParent(parentTurnos.gameObject.transform);
            aux.transform.position += new Vector3(num * 120, 0, 0);
            if (num == 0)
                aux.ActivarTexto();
            num++;
            turnosJugadores.Enqueue(aux);
        }
        gestorAcciones.PreparaTurno();
    }

   
    public void Init()
    {
        gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            nJugadoresPorEquipo = gm.GetNumJugadoresCombate();
            aliados = new Personaje[nJugadoresPorEquipo];
            enemigos = new Personaje[nJugadoresPorEquipo];
        }
    }
    public Personaje[] GetAllAliados() { return aliados; }
    public Personaje[] GetAllEnemigos() { return enemigos; }
    public Personaje GetAliado(int n)  { return aliados[n]; }
    public Personaje GetEnemigo(int n) { return enemigos[n]; }
    public void SetAliado(Personaje p, int n) { aliados[n] = p;  }
    public void SetEnemigo(Personaje p, int n){ enemigos[n] = p; }

    public void SiguienteTurno()
    {
        Turno aux = turnosJugadores.Dequeue();
        aux.transform.position += new Vector3((turnosJugadores.Count+1) * 120, 0, 0);
        aux.DesctivarTexto();
        turnosJugadores.Enqueue(aux);

        foreach (var t in turnosJugadores)
            t.Deslizar();
        turnosJugadores.Peek().ActivarTexto();

        // gestionamos los jugadores muertos, para calaveras y para ver si acabamos la partida
        // aliados
        bool todosMuertos = true;
        for (int i = 0; i < nJugadoresPorEquipo ; i++)
        {
            if (aliados[i].EstaMuerto())
            {
                foreach (var x in turnosJugadores)
                {
                    if (x.GetId() == i)
                        x.PonerCalavera();
                }
            }
            else
                todosMuertos = false;
        }
        if(todosMuertos)
        {
            menuGameOver.SetActive(true);
            return;
        }

        todosMuertos = true;
        // enemigos
        for (int i = 0; i < nJugadoresPorEquipo; i++)
        {
            if (enemigos[i].EstaMuerto())
            {
                foreach (var x in turnosJugadores)
                {
                    if (x.GetId() == i + nJugadoresPorEquipo)
                        x.PonerCalavera();
                }
            }
            else
                todosMuertos = false;
        }
        if (todosMuertos)
            menuVictory.SetActive(true);
    }


    public int GetTurno() { return turnosJugadores.Peek().GetId(); }
    public int GetNumPersonajesPorEquipo() { return nJugadoresPorEquipo; }
    public Personaje GetPersonajeTurno()
    {
        int turno = GetTurno();
        if (turno < nJugadoresPorEquipo)
        {
            return aliados[turno];
        }
        else
        {
            return enemigos[turno - nJugadoresPorEquipo];
        }
    }
    public void Menu()
    {
        menuExit.SetActive(!menuExit.activeSelf);
    }
}
