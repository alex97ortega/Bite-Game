using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public BotonesPlayers[] botones;
    GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        gm.SetNumJugadoresCombate(3);
    }

    public void Seleccionar(int boton, bool aliado)
    {
        botones[boton].PintaBoton(aliado);
        botones[boton].Fijar();
        if (aliado)
            gm.SeleccionarJugadorAliado(botones[boton].personaje);
        else
            gm.SeleccionarJugadorEnemigo(botones[boton].personaje);
    }
    public void Fijar()
    {
        int botonSeleccionado = -1;
        foreach (var b in botones)
        {
            if (b.EstaSeleccionadoPeroNoFijo())
                botonSeleccionado = b.id;
        }
        if (botonSeleccionado == -1)
            return;

        // solo hacemos la elección si hay alguno seleccionado
        botones[botonSeleccionado].Fijar();
        var lobby = FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>();

        if (lobby.IsAliado())
            gm.SeleccionarJugadorAliado(botones[botonSeleccionado].personaje);
        else
            gm.SeleccionarJugadorEnemigo(botones[botonSeleccionado].personaje);

        lobby.SeleccionadoPersonaje(botonSeleccionado);
    }

    public void DeseleccionaAnterior()
    {
        foreach (var b in botones)
        {
            if (b.EstaSeleccionadoPeroNoFijo())
                b.Deselect();
        }
    }

    public void SetEnemigo() { gm.SetEnemigo(); }
}
