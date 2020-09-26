using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dani : Personaje
{
    public Transform cabesa, torso;
    Vector3 initialCabesaScale;
    Vector3 initialTorsoRot;
    float avanzado = 0;

    private void Start()
    {
        initialCabesaScale = cabesa.localScale;
        initialTorsoRot = torso.eulerAngles;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (cabesa.localScale.x < 4)
        {
            panelHp.SetActive(false);
            cabesa.localScale += new Vector3(0.02f, 0.02f, 0.008f);
            cabesa.position += new Vector3(0, 0.007f, 0);
        }
        else
        {
            if (aliado)
            {
                if (avanzado < 80)
                {
                    avanzado += 10;
                    torso.eulerAngles -= new Vector3(10, 0, 0);
                }
                else
                {
                    Restaura();
                    return true;
                }
            }
            //enemigo
            else
            {
                if (avanzado < 80)
                {
                    avanzado += 10;
                    torso.eulerAngles -= new Vector3(10, 0, 0);
                }
                else
                {
                    Restaura();
                    return true;
                }
            }
        }
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
        avanzado = 0;
        torso.eulerAngles = initialTorsoRot;
        cabesa.localScale = initialCabesaScale;
        cabesa.localPosition = new Vector3(cabesa.localPosition.x, 2, cabesa.localPosition.z);
    }
}
