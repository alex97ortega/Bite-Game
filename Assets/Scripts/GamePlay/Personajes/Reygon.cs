using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reygon : Personaje
{
    public Transform torso;
    public GameObject escupitajoPrefab;
    public ParticleSystem pedaco;

    Vector3 initialTorsoRot;
    float avanzado = 0;
    bool creadoEscupitajo = false;

    private void Start()
    {
        initialTorsoRot = torso.eulerAngles;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (avanzado < 0.4f)
        {
            PlaySonidoAM();
            panelHp.SetActive(false);
            
            avanzado += Time.deltaTime;
            torso.eulerAngles -= new Vector3(3, 0, 0);   
        }
        else
        {
            avanzado+= Time.deltaTime;
            if(!creadoEscupitajo)
            {
                GameObject escupitajo = Instantiate(escupitajoPrefab);
                if(aliado)
                    escupitajo.transform.position = objetivo.transform.position + new Vector3(0.5f, 0, 0);
                else
                    escupitajo.transform.position = objetivo.transform.position + new Vector3(-0.5f, 0, 0);
                creadoEscupitajo = true;
            }
            else if(avanzado > 1)
            {
                Restaura();
                objetivo.HacerDanyo(dmgAM);
                FindObjectOfType<Terreno>().EnvenenarCasillas(objetivo.GetCasillaX(), objetivo.GetCasillaZ(), 0, dmgAM);
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
        if(avanzado==0)
        {
            avanzado++;
            PlaySonidoAE();
            ParticleSystem pedo = Instantiate(pedaco);
            pedo.gameObject.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        }
        else
        {
            if(!sonidoAE.isPlaying)
            {
                avanzado = 0;
                jugadaUlti = true;
                FindObjectOfType<Terreno>().EnvenenarCasillas(casillaX, casillaZ, 1, dmgAE);
                return true;
            }
        }
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        creadoEscupitajo = false;
        avanzado = 0;
        torso.eulerAngles = initialTorsoRot;
    }
}
