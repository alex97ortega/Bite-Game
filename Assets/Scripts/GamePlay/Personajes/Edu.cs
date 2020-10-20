using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edu : Personaje
{
    //float avanzado = 0;
    public GameObject palaPrefab;

    GameObject pala;

    private void Start()
    {

    }

    public override bool AnimacionAM(Personaje objetivo)
    {

        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {
        if (pala == null)
        {
            pala = Instantiate(palaPrefab);
            pala.transform.position = transform.position;
        }
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        //avanzado = 0;
    }
}
