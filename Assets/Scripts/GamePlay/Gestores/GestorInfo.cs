using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorInfo : MonoBehaviour
{
    public GestorPartida gestorPartida;
    public GameObject menuInstrucciones, menuAtaques;
    public Text playerInfo, info1, info2, info3;

    public void ShowInfoAtaques()
    {
        if (menuAtaques.activeSelf)
            menuAtaques.SetActive(false);
        else
        {
            menuAtaques.SetActive(true);
            playerInfo.text = gestorPartida.GetPersonajeTurno().nombre;
            info1.text = gestorPartida.GetPersonajeTurno().infoAC;
            info2.text = gestorPartida.GetPersonajeTurno().infoAD;
            info3.text = gestorPartida.GetPersonajeTurno().infoAE;
        }
    }
    public void ShowInstrucciones()
    {
        menuInstrucciones.SetActive(!menuInstrucciones.activeSelf);
    }
}
