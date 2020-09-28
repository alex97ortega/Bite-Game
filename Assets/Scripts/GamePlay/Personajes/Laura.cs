using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laura : Personaje
{
    public Transform torso, brazoIzq, brazoDch;
    Vector3 iniPosDch, iniPosIzq, iniRotIzq, iniRotDch;
    Vector3 iniRotTorso;

    int veces = 0;
    float movido = 60;
    bool ataqueIzq = true;

    private void Start()
    {
        iniPosIzq = brazoIzq.localPosition;
        iniPosDch = brazoDch.localPosition;

        iniRotIzq = brazoIzq.eulerAngles;
        iniRotDch = brazoDch.eulerAngles;


        iniRotTorso = torso.eulerAngles;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        
        if (veces < 20)
        {
            if (veces == 0)
            {
                PlaySonidoAM();
                panelHp.SetActive(false);
            }
            if(ataqueIzq)
            {
                if(movido<120)
                {
                    if(movido>=60)
                    {
                        brazoIzq.localPosition = iniPosIzq + new Vector3(0, 0.35f, 0);
                        brazoIzq.eulerAngles = iniRotIzq + new Vector3(90, 0, 0);
                        brazoDch.localPosition = iniPosDch + new Vector3(0, 0.35f, 0);
                        brazoDch.eulerAngles = iniRotDch + new Vector3(90, 0, 0);
                    }
                    movido+= 500*Time.deltaTime;
                    torso.eulerAngles += new Vector3(0, 10, 0);
                }
                else
                {
                    veces++;
                    movido = 0;
                    ataqueIzq = false;
                    brazoIzq.localPosition = iniPosIzq;
                    brazoIzq.eulerAngles = iniRotIzq;
                    brazoDch.localPosition = new Vector3(-0.6f, 2.7f, 0);
                    brazoDch.eulerAngles = new Vector3(180, 0, 0);
                }
            }
            else
            {
                if (movido < 120)
                {
                    if (movido >= 60)
                    {
                        brazoIzq.localPosition = iniPosIzq + new Vector3(0, 0.35f, 0);
                        brazoIzq.eulerAngles = iniRotIzq + new Vector3(90, 0, 0);
                        brazoDch.localPosition = iniPosDch + new Vector3(0, 0.35f, 0);
                        brazoDch.eulerAngles =iniRotDch +new Vector3(90, 0, 0);
                    }
                    movido +=500*Time.deltaTime;
                    torso.eulerAngles -= new Vector3(0, 10, 0);
                }
                else
                {
                    veces++;
                    movido = 0;
                    ataqueIzq = true;
                    brazoIzq.localPosition += new Vector3(0, 0.35f, 0);
                    brazoIzq.eulerAngles += new Vector3(90, 0, 0);
                    brazoDch.localPosition = iniPosDch;
                    brazoDch.eulerAngles = iniRotDch;
                }
            }
        }
        else
        {
            Restaura();
            objetivo.HacerDanyo(dmgAM);
            return true;
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
        ataqueIzq = true;
        movido = 60;
        veces = 0;
        torso.eulerAngles = iniRotTorso;
        brazoIzq.localPosition = iniPosIzq;
        brazoIzq.eulerAngles = iniRotIzq;

        brazoDch.localPosition = iniPosDch;
        brazoDch.eulerAngles = iniRotDch;
        panelHp.SetActive(true);
    }
}
