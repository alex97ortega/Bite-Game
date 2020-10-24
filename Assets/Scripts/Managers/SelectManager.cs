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

    // esta mierda no pinta nada aqui, pero como no se puede 
    // acceder al GM desde el AutoLobby ps es lo k hay
    public void SetTipoPartidaMultiplayer(int numJugadores)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            switch (numJugadores)
            {
                case 2:
                    gm.SetTipoPartida(GameManager.TipoPartida.PARTIDA_MULTIP_1VS1);
                    break;
                case 6:
                    gm.SetTipoPartida(GameManager.TipoPartida.PARTIDA_MULTIP_3VS3);
                    break;
                default:
                    gm.SetTipoPartida(GameManager.TipoPartida.PARTIDA_SOLO_PLAYER);
                    break;
            }
        }
    }
}
