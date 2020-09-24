using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorInfo : MonoBehaviour
{
    public GestorPartida gestorPartida;
    public GameObject menuInfo;
    public Text playerInfo, info1, info2, info3;

    public void ShowInfo()
    {
        if (menuInfo.activeSelf)
            menuInfo.SetActive(false);
        else
        {
            menuInfo.SetActive(true);
            playerInfo.text = gestorPartida.GetPersonajeTurno().nombre;
            info1.text = gestorPartida.GetPersonajeTurno().infoAC;
            info2.text = gestorPartida.GetPersonajeTurno().infoAD;
            info3.text = gestorPartida.GetPersonajeTurno().infoAE;
        }
    }
}
