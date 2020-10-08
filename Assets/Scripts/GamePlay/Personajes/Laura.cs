using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laura : Personaje
{
    public Transform torso, hombroIzq, hombroDch;
    Vector3 iniRotIzq, iniRotDch;
    Vector3 iniRotTorso;

    int veces = 0;
    float movido = 60, avanzado = 0;
    bool ataqueIzq = true;
    bool seCallo = false;

    private void Start()
    {
        iniRotIzq = hombroIzq.eulerAngles;
        iniRotDch = hombroDch.eulerAngles;


        iniRotTorso = torso.eulerAngles;
    }
    private void Update()
    {
        if(jugadaUlti && !muerto && !seCallo && !sonidoAE.isPlaying)
        {
            seCallo = true;
            log.LanzaLog("Habéis conseguido aguantar sin matarla hasta que se calló, enhorabuena!!");
        }
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (veces == 0 && !sonidoAM.isPlaying)
        {
            PlaySonidoAM();
            panelHp.SetActive(false);
        }
        else
        {
            if(ataqueIzq)
            {
                if(movido<120)
                {
                    if(movido>=60)
                    {
                        hombroIzq.eulerAngles = iniRotIzq + new Vector3(70, 0, 0);
                        hombroDch.eulerAngles = iniRotDch + new Vector3(70, 0, 0);
                    }
                    movido+= 500*Time.deltaTime;
                    torso.eulerAngles += new Vector3(0, 500 * Time.deltaTime, 0);
                }
                else
                {
                    veces++;
                    movido = 0;
                    ataqueIzq = false;
                    hombroIzq.eulerAngles = iniRotIzq;
                    hombroDch.eulerAngles += new Vector3(70, 0, 0);
                }
            }
            else
            {
                if (movido < 120)
                {
                    if (movido >= 60)
                    {
                        hombroIzq.eulerAngles = iniRotIzq + new Vector3(70, 0, 0);
                        hombroDch.eulerAngles =iniRotDch +new Vector3(70, 0, 0);
                    }
                    movido +=500*Time.deltaTime;
                    torso.eulerAngles -= new Vector3(0, 500 * Time.deltaTime, 0);
                }
                else
                {
                    veces++;
                    movido = 0;
                    ataqueIzq = true;
                    hombroIzq.eulerAngles += new Vector3(70, 0, 0);
                    hombroDch.eulerAngles = iniRotDch;
                }
            }
            if(!sonidoAM.isPlaying)
            {
                Restaura();
                if (objetivo.nombre == "Dani" || objetivo.nombre == "Reygon")
                {
                    log.LanzaLog("Lobezno ha destripado sanguinariamente a " + objetivo.nombre + ". Es muy eficaz!!");
                    objetivo.HacerDanyo(dmgAM * bonifDmg *2);
                }
                else
                {
                    log.LanzaLog("Lobezno ha entrado en combate.");
                    objetivo.HacerDanyo(dmgAM * bonifDmg);
                }
                return true;
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
        if(!sonidoAE.isPlaying)
        {
            PlaySonidoAE();
        }
        avanzado += Time.deltaTime;
        if(avanzado>=3.8f)
        {
            Restaura();
            jugadaUlti = true;
            log.LanzaLog("¿Alguien tiene unos tapones para los oídos?");
            return true;
        }
        return false;
    }

    private void Restaura()
    {
        ataqueIzq = true;
        movido = 60;
        veces = 0;
        avanzado = 0;
        torso.eulerAngles = iniRotTorso;

        hombroIzq.eulerAngles = iniRotIzq;
        hombroDch.eulerAngles = iniRotDch;
        panelHp.SetActive(true);
    }
}
