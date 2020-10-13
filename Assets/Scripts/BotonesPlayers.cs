using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesPlayers : MonoBehaviour
{
    public Personaje personaje;
    public int id;
    public GameObject tick, tickAzul, tickRojo;
    GameManager gm;
    bool fijo = false;

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
        Select(FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>().IsAliado());
    }

    public void Select(bool aliado)
    {
        if (!FindObjectOfType<Photon.Pun.Demo.PunBasics.AutoLobby>().PuedeSeleccionar())
            return;

        if (fijo)
            return;

        FindObjectOfType<SelectManager>().DeseleccionaAnterior();

        PintaBoton(aliado);
    }

    public void PintaBoton(bool aliado)
    {
        if (aliado && !tickRojo.activeSelf)
        {
            if (!tickAzul.activeSelf)
            {
                tickAzul.SetActive(true);
            }
        }
        else if (!aliado && !tickAzul.activeSelf)
        {
            if (!tickRojo.activeSelf)
            {
                tickRojo.SetActive(true);
            }
        }
    }

    public void Deselect()
    {
        if (fijo)
            return;
        tickAzul.SetActive(false);
        tickRojo.SetActive(false);
    }

    public void Fijar()
    {
        if (tickAzul.activeSelf || tickRojo.activeSelf)
            fijo = true;
    }
    public bool EstaSeleccionadoPeroNoFijo()
    {
        return !fijo && (tickAzul.activeSelf || tickRojo.activeSelf);
    }
}
