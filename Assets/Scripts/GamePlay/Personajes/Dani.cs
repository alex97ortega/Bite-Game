using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dani : Personaje
{
    public Transform cabesa, torso, hombroDch;
    public GameObject aquariusPrefab, culturistaPrefab;

    GameObject aquarius, culturista;
    AudioSource musicaFondo;
    Vector3 initialCabesaScale;
    Vector3 initialTorsoRot, initialHombroRot;
    float avanzado = 0;

    private void Start()
    {
        musicaFondo = FindObjectOfType<Camara>().GetComponentInChildren<AudioSource>();
        initialCabesaScale = cabesa.localScale;
        initialTorsoRot = torso.eulerAngles;
        initialHombroRot = hombroDch.eulerAngles;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (cabesa.localScale.x < 4.5f)
        {
            musicaFondo.Pause();
            PlaySonidoAM();
            panelHp.SetActive(false);
            cabesa.localScale += new Vector3(0.4f*Time.deltaTime, 0.4f * Time.deltaTime, 0.16f * Time.deltaTime);
            cabesa.position += new Vector3(0, 0.15f * Time.deltaTime, 0);
        }
        else
        {
            if (avanzado < 80)
            {
                avanzado +=10;
                torso.eulerAngles -= new Vector3(10, 0, 0);
            }
            else
            {
                Restaura();
                objetivo.HacerDanyo(dmgAM);
                musicaFondo.Play();
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
        if(avanzado < 1 && aquarius==null)
        {
            PlaySonidoAE();
            aquarius = Instantiate(aquariusPrefab);
            if(aliado)
            {
                aquarius.transform.position = transform.position + new Vector3(0, 1.6f, 1.1f);
                aquarius.transform.eulerAngles += new Vector3(25, 0, 0);
            }
            else
            {
                aquarius.transform.position = transform.position + new Vector3(0, 1.6f, -1.1f);
                aquarius.transform.eulerAngles -= new Vector3(25, 0, 0);
            }
            aquarius.transform.parent = hombroDch.transform;
        }
        else 
        {
            avanzado += Time.deltaTime;
            if(avanzado > 7.5f && avanzado < 9.5f)
            {
                hombroDch.Rotate(40 * Time.deltaTime, 0, 75 * Time.deltaTime);
                if(aliado)
                    aquarius.transform.Rotate(40 * Time.deltaTime, 0, 120 * Time.deltaTime);
                else
                    aquarius.transform.Rotate(-40 * Time.deltaTime, 0, -120 * Time.deltaTime);
            }
            else if (avanzado > 13.8f && culturista==null)
            {
                float guardavanzado = avanzado;
                Restaura();
                avanzado = guardavanzado;

                culturista = Instantiate(culturistaPrefab);
                if(aliado)
                    culturista.transform.position = transform.position + new Vector3(-0.3f, 1.2f, 0);
                else
                {
                    culturista.transform.position = transform.position + new Vector3(0.3f, 1.2f, 0);
                    culturista.transform.eulerAngles = new Vector3(90, 180, 90);
                }
            }
            if (!sonidoAE.isPlaying)
            {
                jugadaUlti = true;
                Destroy(culturista);
                return true;
            }
        }
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        avanzado = 0;
        torso.eulerAngles = initialTorsoRot;
        cabesa.localScale = initialCabesaScale;
        hombroDch.eulerAngles = initialHombroRot;
        cabesa.localPosition = new Vector3(cabesa.localPosition.x, 2, cabesa.localPosition.z);
        if (aquarius != null)
            Destroy(aquarius);
    }
}
