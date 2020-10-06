using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reygon : Personaje
{
    public Transform torso;
    public GameObject escupitajoPrefab, microfonoPrefab;
    public ParticleSystem pedaco;

    Vector3 initialTorsoRot;
    float avanzado = 0;
    bool creadoEscupitajo = false;
    GameObject microfono;

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
                log.LanzaLog("Reygon creó una piscina artificial debajo de " + objetivo.nombre + ".");
                objetivo.HacerDanyo(dmgAM * bonifDmg);
                FindObjectOfType<Terreno>().EnvenenarCasillas(objetivo.GetCasillaX(), objetivo.GetCasillaZ(), 0, dmgAM);
                return true;
            }
        }
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if(microfono == null)
        {
            transform.position += new Vector3(0, 1000, 0);
            objetivo.PlayCancion();
            microfono = Instantiate(microfonoPrefab);
            microfono.transform.position = objetivo.transform.position;
            if (!aliado)
                microfono.transform.eulerAngles += new Vector3(0, 180, 0);
        }
        else if(objetivo.CancionFinished())
        {
            transform.position -= new Vector3(0, 1000, 0);
            Restaura();
            ultimaJugoAD = true;
            log.LanzaLog(objetivo.nombre + " acaba de cantar una preciosa canción encima de Microfonomán.");
            foreach (var p in FindObjectOfType<GestorPartida>().GetAllEnemigos())
                p.HacerDanyo(dmgAD * bonifDmg);
            foreach (var p in FindObjectOfType<GestorPartida>().GetAllAliados())
                p.HacerDanyo(dmgAD * bonifDmg);
            return true;
        }
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
                log.LanzaLog("Yo no me acercaría mucho a ese menda.");
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
        if (microfono != null)
            Destroy(microfono);
    }
}
