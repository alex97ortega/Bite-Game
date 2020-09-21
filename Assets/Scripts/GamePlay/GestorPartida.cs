using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorPartida : MonoBehaviour
{
    public GameObject menuAcciones;
    public Turno turnoPrefab;
    public Canvas canvas;

    Personaje[] aliados;
    Personaje[] enemigos;
    Queue<Turno> turnosJugadores;
    GameManager gm;
    bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        Queue<int> turnos = new Queue<int>();
        turnosJugadores = new Queue<Turno>();

        for (int i = 0; i< gm.GetNumJugadoresCombate() * 2; i++)
        {
            int x;
            int veces = 0;
            do
            {
                veces++;
                x = Random.Range(0, gm.GetNumJugadoresCombate() * 2);
            } while (turnos.Contains(x) && veces < 1000);
            turnos.Enqueue(x);
        }
        int num = 0;

        foreach(int x in turnos)
        {
            Turno aux = Instantiate(turnoPrefab);
            if (x < gm.GetNumJugadoresCombate())
                aux.SetProperties(x, aliados[x].GetFoto());
            else
                aux.SetProperties(x, enemigos[x - gm.GetNumJugadoresCombate()].GetFoto());
            aux.transform.SetParent(canvas.gameObject.transform);
            aux.transform.position += new Vector3(num * 120, 0, num);
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
        if(turnosJugadores.Peek().GetId() >= gm.GetNumJugadoresCombate())
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
        if (initialized)
            return;

        gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            aliados = new Personaje[gm.GetNumJugadoresCombate()];
            enemigos = new Personaje[gm.GetNumJugadoresCombate()];
            initialized = true;
        }
    }
    public void SetAliado(Personaje p, int n)
    {
        aliados[n] = p;
    }
    public Personaje GetAliado(int n) { return aliados[n]; }
    public void SetEnemigo(Personaje p, int n)
    {
        enemigos[n] = p;
    }
    public Personaje GetEnemigo(int n) { return enemigos[n]; }
}
