using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yaiza : Personaje
{
    //float avanzado = 0;

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
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        //avanzado = 0;
    }
}
