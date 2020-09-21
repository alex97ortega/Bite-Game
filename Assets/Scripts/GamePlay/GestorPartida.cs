using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorPartida : MonoBehaviour
{
    public GameObject menuAcciones, menuExit;
    public Turno turnoPrefab;
    public Canvas canvas;
    public RandomEnemies randomEnemies;

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
        int num = 0;

        foreach(int x in turnos)
        {
            Turno aux = Instantiate(turnoPrefab);
            if (x < nJugadoresPorEquipo)
                aux.SetProperties(x, aliados[x].GetFoto());
            else
                aux.SetProperties(x, enemigos[x - nJugadoresPorEquipo].GetFoto());
            aux.transform.SetParent(canvas.gameObject.transform);
            aux.transform.position += new Vector3(num * 120, 0, 0);
            if (num == 0)
                aux.ActivarTexto();
            num++;
            turnosJugadores.Enqueue(aux);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // gestion de la "IA"
        // un numero mayor que el numero de personajes por bando significa que pertenece
        // al bando enemigos. Si no, al de aliados
        if(GetTurno() >= nJugadoresPorEquipo)
        {
            menuAcciones.SetActive(false);
        }
        else
        {
            menuAcciones.SetActive(true);
        }
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
    public Personaje GetAliado(int n)  { return aliados[n]; }
    public Personaje GetEnemigo(int n) { return enemigos[n]; }
    public void SetAliado(Personaje p, int n) { aliados[n] = p;  }
    public void SetEnemigo(Personaje p, int n){ enemigos[n] = p; }

    public void PasarTurno()
    {
        Turno aux = turnosJugadores.Dequeue();
        aux.transform.position += new Vector3((turnosJugadores.Count+1) * 120, 0, 0);
        aux.DesctivarTexto();
        turnosJugadores.Enqueue(aux);

        foreach (var t in turnosJugadores)
            t.Deslizar();
        turnosJugadores.Peek().ActivarTexto();
    }
    int GetTurno() { return turnosJugadores.Peek().GetId(); }
    Personaje GetPersonajeTurno()
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
