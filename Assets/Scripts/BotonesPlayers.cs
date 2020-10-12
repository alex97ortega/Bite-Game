using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesPlayers : MonoBehaviour
{
    public Personaje personaje;
    public int id;
    public GameObject tick, tickAzul, tickRojo;
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void SelectOffiline()
    {
        // seleccionar
        if (!tick.activeSelf)
        {
            tick.SetActive(true);
            if (gm)
                gm.SeleccionarJugador(personaje);
        }
        // deseleccionar
        else
        {
            tick.SetActive(false);
            if (gm)
                gm.DeseleccionarJugador(personaje);
        }
    }

    public void SelectOnline()
    {
        Select(FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>().IsAliado(), true);
    }

    public void Select(bool aliado, bool enviarMensajes)
    {
        if (aliado && !tickRojo.activeSelf)
        {
            if (!tickAzul.activeSelf)
            {
                tickAzul.SetActive(true);
                if (enviarMensajes)
                    FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>().SeleccionaAliado(id);
            }
            else
            {
                tickAzul.SetActive(false);
                if (enviarMensajes)
                    FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>().DeseleccionaAliado(id);
            }
        }
        else if (!aliado && !tickAzul.activeSelf)
        {
            if (!tickRojo.activeSelf)
            {
                tickRojo.SetActive(true);
                if (enviarMensajes)
                    FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>().SeleccionaEnemigo(id);
            }
            else
            {
                tickRojo.SetActive(false);
                if (enviarMensajes)
                    FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>().DeseleccionaEnemigo(id);
            }
        }
    }
}
