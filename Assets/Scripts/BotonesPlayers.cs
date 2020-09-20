using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesPlayers : MonoBehaviour
{
    public Personaje personaje;
    public GameObject tick;
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void Select()
    {
        // seleccionar
        if(!tick.activeSelf)
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
}
