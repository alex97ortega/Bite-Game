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
    public GestorMultiplayer gestorMultiplayer;
    public Terreno tablero;

    Personaje[] aliados;
    Personaje[] enemigos;
    Queue<int> turnos;
    Queue<Turno> turnosJugadores;
    GameManager gm;
    int nJugadoresPorEquipo;

    // Start is called before the first frame update
    void Start()
    {
        randomEnemies.GenerateEnemies();

        turnosJugadores = new Queue<Turno>();
        turnos = new Queue<int>();
        if (!gestorMultiplayer.gameObject.activeSelf || gestorMultiplayer.EsAnfitrion())
        {
            turnos = SeleccionAleatoria();
            // ahora sí vamos creando los personajes
            CreaTurnos(turnos);
        }
    }

    // seleccion aleatoria de turnos
    private Queue<int> SeleccionAleatoria()
    {
        for (int i = 0; i < nJugadoresPorEquipo * 2; i++)
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
        return turnos;
    }
   
    // crea los iconos de los turnos y la cola
    public void CreaTurnos(Queue<int> ordenTurnos)
    {
        int num = 0;
        foreach (int x in ordenTurnos)
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
    public Personaje GetPersonaje(string nombre)
    {
        if (nombre == "")
            return null;

        for (int n = 0; n < nJugadoresPorEquipo * 2; n++)
        {
            if (n < nJugadoresPorEquipo && aliados[n].nombre==nombre)
            {
                 return aliados[n];
            }
            else if(n >= nJugadoresPorEquipo && enemigos[n - nJugadoresPorEquipo].nombre == nombre)
            {
                 return enemigos[n - nJugadoresPorEquipo];
            }
        }
        return null;
    }
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
            if(gm.IsAliado())
                menuGameOver.SetActive(true);
            else
                menuVictory.SetActive(true);
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
        {
            if (gm.IsAliado())
                menuVictory.SetActive(true);
            else
                menuGameOver.SetActive(true);
        }
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
    public bool Ready() { return turnosJugadores.Count > 0; } 
    public void AddTurno(int turno)
    {
        turnos.Enqueue(turno);
        if(turnos.Count==6)
            CreaTurnos(turnos);
    }
    public void LanzaMensajeTurnos()
    {
        if (gestorMultiplayer.EsAnfitrion())
        {
            foreach (var x in turnos)
                gestorMultiplayer.SendOrdenTurnos(x);
            int[] posAliadosZ = new int[nJugadoresPorEquipo];
            int[] posEnemigosZ = new int[nJugadoresPorEquipo];
            for (int i = 0; i < enemigos.Length; i++)
            {
                posAliadosZ[i] = aliados[i].GetCasillaZ();
                posEnemigosZ[i] = enemigos[i].GetCasillaZ();
            }
            gestorMultiplayer.SendPosicionesAliados(posAliadosZ);
            gestorMultiplayer.SendPosicionesEnemigos(posEnemigosZ);
        }
    }

    public void ColocaAliados(int[] posicionesZ)
    {
        for(int i = 0; i < aliados.Length;i++)        
        {
            tablero.GetCasilla(aliados[i].GetCasillaX(), aliados[i].GetCasillaZ()).Desocupar();
            tablero.GetCasilla(aliados[i].GetCasillaX(), posicionesZ[i]).Ocupar(aliados[i]);
            aliados[i].SetPos(aliados[i].GetCasillaX(), posicionesZ[i]);
        }
        gestorAcciones.PreparaTurno();
    }
    public void ColocaEnemigos(int[] posicionesZ)
    {
        for (int i = 0; i < enemigos.Length; i++)
        {
            tablero.GetCasilla(enemigos[i].GetCasillaX(), enemigos[i].GetCasillaZ()).Desocupar();
            tablero.GetCasilla(enemigos[i].GetCasillaX(), posicionesZ[i]).Ocupar(enemigos[i]);
            enemigos[i].SetPos(enemigos[i].GetCasillaX(), posicionesZ[i]);
        }
        gestorAcciones.PreparaTurno();
    }
}
