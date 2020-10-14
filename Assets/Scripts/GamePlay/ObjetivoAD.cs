using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetivoAD : MonoBehaviour
{
    public Image foto, calavera, inmune;
    Personaje personaje;

    public void SetProperties(Personaje p, Sprite sp)
    {
        personaje = p;
        foto.sprite = sp;
    }

    public void ActualizaEnemigo()
    {
        if(personaje.EstaMuerto())
        {
            calavera.gameObject.SetActive(true);
            return;
        }
        inmune.gameObject.SetActive(personaje.EsInmune());
    }

    public void Seleccionado()
    {
        if (calavera.gameObject.activeSelf || inmune.gameObject.activeSelf)
            return ;
        FindObjectOfType<GestorAcciones>().ConfirmadoObjetivoAD(personaje.nombre, true);
    }
}
