using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instrucciones : MonoBehaviour
{
    public Image[] instructions;
    public Image flechaIzq, flechaDcha;

    int pagActual = 0;

    public void PasaIzquierda()
    {
        instructions[pagActual].gameObject.SetActive(false);
        pagActual--;
        instructions[pagActual].gameObject.SetActive(true);
        if (pagActual == 0)
            flechaIzq.gameObject.SetActive(false);
        if(pagActual == instructions.Length-2)
            flechaDcha.gameObject.SetActive(true);
    }

    public void PasaDerecha()
    {
        instructions[pagActual].gameObject.SetActive(false);
        pagActual++;
        instructions[pagActual].gameObject.SetActive(true);
        if (pagActual == 1)
            flechaIzq.gameObject.SetActive(true);
        if (pagActual == instructions.Length - 1)
            flechaDcha.gameObject.SetActive(false);
    }
}
