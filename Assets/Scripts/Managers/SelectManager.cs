using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public BotonesPlayers[] botones;

    public void Seleccionar(int boton, bool aliado)
    {
        botones[boton].Select(aliado, false);
    }
}
