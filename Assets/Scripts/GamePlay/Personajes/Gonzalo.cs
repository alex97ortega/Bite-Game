using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gonzalo : Personaje
{
    public Transform body;
    Vector3 initialBodyScale;
    Vector3 initialBodyRotation;
    Vector3 guardaPos;
    float avanzado = 0;

    private void Start()
    {
        initialBodyScale = body.localScale;
        initialBodyRotation = body.eulerAngles;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (body.localScale.x < 4)
        {
            body.localScale += new Vector3(0.05f, 0.043f, 0.055f);
            panelHp.SetActive(false);
            guardaPos = transform.position;
        }
        else
        {
            if(aliado)
            {
                if(avanzado < 6)
                {
                    avanzado += 0.08f;
                    transform.position -= new Vector3(0.08f, 0, 0);
                    body.eulerAngles -= new Vector3(10, 0, 0);
                    if (avanzado > 2)
                    {
                        objetivo.RestauraPropiedades();
                        objetivo.transform.position += new Vector3(1.5f, 0.2f, 0);
                        objetivo.transform.eulerAngles += new Vector3(90, 0, 0);
                    }
                }
                else
                {
                    Restaura();
                    objetivo.RestauraPropiedades();
                    return true;
                }
            }
            //enemigo
            else
            {
                if (avanzado < 6)
                {
                    avanzado += 0.08f;
                    transform.position += new Vector3(0.08f, 0, 0);
                    body.eulerAngles += new Vector3(10, 0, 0);
                    if(avanzado > 2)
                    {
                        objetivo.RestauraPropiedades();
                        objetivo.transform.position += new Vector3(-1.5f, 0.2f, 0);
                        objetivo.transform.eulerAngles += new Vector3(90, 0, 0);
                    }
                }
                else
                {
                    Restaura();
                    objetivo.RestauraPropiedades();
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
        avanzado = 0;
        transform.position = guardaPos;
        body.transform.localScale = initialBodyScale;
        body.transform.eulerAngles = initialBodyRotation;
        panelHp.SetActive(true);
    }
}
