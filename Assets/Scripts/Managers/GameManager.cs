﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numJugadoresCombate;
    private int numJugadoresSeleccionados;

    List<Personaje> jugadoresPartidaAliados;
    List<Personaje> jugadoresPartidaEnemigos;
    bool aliado = true;

    //singletone
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Make sure session manager persists between scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // always keep current session manager instead of the one in the scene.
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        numJugadoresCombate = 0;
        numJugadoresSeleccionados = 0;
        jugadoresPartidaAliados = new List<Personaje>();
        jugadoresPartidaEnemigos = new List<Personaje>();
        aliado = true;
    }

    public int GetNumJugadoresCombate() { return numJugadoresCombate; }
    public int GetNumJugadoresSeleccionados() { return numJugadoresSeleccionados; }

    public void SetNumJugadoresCombate(int n) {
        numJugadoresCombate = n;
        numJugadoresSeleccionados = 0;
        jugadoresPartidaAliados = new List<Personaje>();
        jugadoresPartidaEnemigos = new List<Personaje>();
    }

    // para menu de seleccion
    public void SeleccionarJugador(Personaje personaje)
    {
        jugadoresPartidaAliados.Add(personaje);
        numJugadoresSeleccionados++;
        if (numJugadoresSeleccionados == numJugadoresCombate)
        {
            SceneManager.LoadScene("GamePlay");
        }
    }

    public void DeseleccionarJugador(Personaje personaje)
    {
        jugadoresPartidaAliados.Remove(personaje);        
        numJugadoresSeleccionados--;
    }

    public List<Personaje> GetJugadoresPartida() { return jugadoresPartidaAliados; }
    public List<Personaje> GetJugadoresEnemigos() { return jugadoresPartidaEnemigos; }

    public void SeleccionarJugadorAliado(Personaje personaje)
    {
        jugadoresPartidaAliados.Add(personaje);
    }
    public void SeleccionarJugadorEnemigo(Personaje personaje)
    {
        jugadoresPartidaEnemigos.Add(personaje);
    }

    public bool IsAliado() { return aliado; }
    public void SetEnemigo() { aliado = false; }
}
