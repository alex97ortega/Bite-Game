﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alex : Personaje
{
    public GameObject palomitasPrefab, microondasPrefab, apioPrefab, sillaPrefab, ordenadorPrefab;
    public Transform piernas, rodillas, brazoIzq, brazoDch;
    public AudioSource sonidoMicro;
    float avanzado = 0;
    GameObject palomitas, microondas, apio, silla, ordenador;
    bool sentado = false;
    
    public override bool AnimacionAM(Personaje objetivo)
    {
        if (avanzado < 1)
        {
            panelHp.SetActive(false);
            PlaySonidoAM();
            avanzado += Time.deltaTime;
        }
        else
        {

            if (palomitas==null)
            {
                palomitas = Instantiate(palomitasPrefab);
                palomitas.transform.position = objetivo.transform.position + new Vector3(0, 1.5f ,0);
                if (!aliado)
                    palomitas.transform.eulerAngles = new Vector3(0, -50, 0);
                objetivo.gameObject.SetActive(false);
                avanzado = 0.5f;
            }
            else if(palomitas.transform.localScale.y > 1)
            {
                palomitas.transform.localScale -= new Vector3(0, 3*Time.deltaTime, 3 * Time.deltaTime);
                palomitas.transform.position -= new Vector3(0, 1.5f * Time.deltaTime, 0);
            }
            else if(microondas == null)
            {
                microondas = Instantiate(microondasPrefab);
                microondas.transform.position = objetivo.transform.position + new Vector3(0, 1, 0);
                if (!aliado)
                    microondas.transform.eulerAngles = new Vector3(0, -240, 0);
                sonidoMicro.Play();
            }
            else
            {
                avanzado+= Time.deltaTime;
                if(avanzado>3.5f)
                {
                    objetivo.gameObject.SetActive(true);
                    objetivo.HacerDanyo(dmgAM);
                    Restaura();
                    return true;
                }
            }
        }
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if(apio== null)
        {
            apio = Instantiate(apioPrefab);
            if(aliado)
            {
                apio.transform.position = transform.position + new Vector3(0,2.1f, -1.1f);
                apio.transform.eulerAngles += new Vector3(15, 0, 0);
            }
            else
            {
                apio.transform.position = transform.position + new Vector3(0, 2.1f, 1.1f);
                apio.transform.eulerAngles += new Vector3(-15, 0, 0);
            }
        }
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {
        if (avanzado < 0.5f)
        {
            if(silla == null)
            {
                panelHp.SetActive(false);
                PlaySonidoAE();
                silla = Instantiate(sillaPrefab);
                silla.transform.position = transform.position;
                if(aliado)
                {
                    transform.position -= new Vector3(0.2f, 0, 0);
                }
                else
                {
                    silla.transform.eulerAngles += new Vector3(0, 180, 0);
                    transform.position += new Vector3(0.2f, 0, 0);
                }

            }
            avanzado += Time.deltaTime;
        }
        else
        {
            if(!sentado)
            {
                sentado = true;
                piernas.eulerAngles += new Vector3(75, 0, 0);
                rodillas.eulerAngles += new Vector3(-50, 0, 0);
                if(aliado)
                    transform.position += new Vector3(0.7f, -0.5f, 0);
                else
                    transform.position += new Vector3(-0.7f, -0.5f, 0);
            }
            else if(!sonidoAE.isPlaying)
            {
                piernas.eulerAngles += new Vector3(-75, 0, 0);
                rodillas.eulerAngles += new Vector3(50, 0, 0);

                brazoDch.eulerAngles -= new Vector3(0, -80, 0);
                brazoIzq.eulerAngles -= new Vector3(0, 80, 0);

                if (aliado)
                {
                    transform.position += new Vector3(-0.5f, 0.5f, 0);

                    brazoDch.position -= new Vector3(-0.4f, 0, -0.3f);
                    brazoIzq.position -= new Vector3(-0.4f, 0, 0.3f);

                    foreach (var p in FindObjectOfType<GestorPartida>().GetAllEnemigos())
                        p.HacerDanyo(dmgAE);
                    foreach (var p in FindObjectOfType<GestorPartida>().GetAllAliados())
                        p.Curar(dmgAE);
                }
                else
                {
                    transform.position += new Vector3(0.5f, 0.5f, 0);

                    brazoDch.position -= new Vector3(0.4f, 0, 0.3f);
                    brazoIzq.position -= new Vector3(0.4f, 0, -0.3f);

                    foreach (var p in FindObjectOfType<GestorPartida>().GetAllAliados())
                        p.HacerDanyo(dmgAE);
                    foreach (var p in FindObjectOfType<GestorPartida>().GetAllEnemigos())
                        p.Curar(dmgAE);
                }
                

                Restaura();
                jugadaUlti = true;
                return true;
            }
            if(ordenador==null && avanzado>3)
            {
                ordenador = Instantiate(ordenadorPrefab);
                ordenador.transform.position = transform.position;

                brazoDch.eulerAngles += new Vector3(0, -80, 0);
                brazoIzq.eulerAngles += new Vector3(0, 80, 0);

                if (aliado)
                {
                    brazoDch.position += new Vector3(-0.4f, 0, -0.3f);
                    brazoIzq.position += new Vector3(-0.4f, 0, 0.3f);
                }
                else
                {
                    ordenador.transform.eulerAngles += new Vector3(0, 180, 0);
                    brazoDch.position += new Vector3(0.4f, 0, 0.3f);
                    brazoIzq.position += new Vector3(0.4f, 0, -0.3f);
                }
           
            }
            avanzado += Time.deltaTime;
        }
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        if (palomitas != null)
            Destroy(palomitas);
        if (microondas != null)
            Destroy(microondas);
        if (silla != null)
            Destroy(silla);
        if (ordenador != null)
            Destroy(ordenador);

        avanzado = 0;
        sentado = false;
    }
}
