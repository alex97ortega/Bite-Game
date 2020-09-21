using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu, menuNumeroJugadores, instrucciones;

    public void MostrarMenuNumeroJugadores()
    {
        mainMenu.SetActive(false);
        menuNumeroJugadores.SetActive(true);
    }
    public void MostrarInstrucciones()
    {
        mainMenu.SetActive(false);
        instrucciones.SetActive(true);
    }
    public void VolverMenuPrincipal()
    {
        mainMenu.SetActive(true);
        instrucciones.SetActive(false);
        menuNumeroJugadores.SetActive(false);
    }
}
