using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asier : Personaje
{
    public GameObject paredPrefab, cochePrefab;
    public Transform hombroIzq, hombroDch, manoDch;
    public AudioSource golpe;
    
    float avanzado = 0;
    bool rotandoDch = true;
    GameObject rocodromo, coche;
    AudioSource musicaFondo;

    private void Start()
    {
        musicaFondo = FindObjectOfType<Camara>().GetComponentInChildren<AudioSource>();
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (rocodromo == null)
        {
            musicaFondo.Pause();
            PlaySonidoAM();
            panelHp.SetActive(false);
            objetivo.panelHp.SetActive(false);

            rocodromo = Instantiate(paredPrefab);
            rocodromo.transform.position = transform.position;

            if(aliado)
            {
                transform.position += new Vector3(-3.8f, 1.5f, 0);
                objetivo.transform.position += new Vector3(-0.7f, 0, 1);
            }
            else
            {
                transform.position += new Vector3(3.8f, 1.5f, 0);
                objetivo.transform.position += new Vector3(0.7f, 0, -1);
                rocodromo.transform.Rotate(0, 180, 0);
            }
            hombroIzq.transform.eulerAngles += new Vector3(-20, 0, 90);
            objetivo.transform.parent = manoDch.transform;
        }
        else
        {
            if(rotandoDch)
            {
                avanzado +=  10*Time.deltaTime;
                transform.eulerAngles += new Vector3(0, 0, 25 * Time.deltaTime);
                if(avanzado > 1)
                {
                    avanzado = 1;
                    rotandoDch = false;
                }
            }
            else
            {
                avanzado -=  10*Time.deltaTime;
                transform.eulerAngles -= new Vector3(0, 0, 25 * Time.deltaTime);
                if (avanzado < -1)
                {
                    avanzado = -1;
                    rotandoDch = true;
                }
            }

            if (transform.position.y < 10)
                transform.position += new Vector3(0, 1.5f * Time.deltaTime, 0);
            else
            {
                if (objetivo.transform.position.y > 2.5f)
                {
                    if(objetivo.transform.parent != null)
                    {
                        objetivo.transform.parent = null;
                        objetivo.transform.eulerAngles = new Vector3(objetivo.transform.eulerAngles.x, objetivo.transform.eulerAngles.y, 180);
                        objetivo.transform.position += new Vector3(0, 2, 0);
                    }
                    objetivo.transform.position -= new Vector3(0, 10 * Time.deltaTime, 0);
                }
                else
                {
                    golpe.Play();
                    if (aliado)
                    {
                        transform.position -= new Vector3(-3.8f, 0, 0);
                    }
                    else
                    {
                        transform.position -= new Vector3(3.8f, 0, 0);
                    }
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                    hombroIzq.transform.eulerAngles -= new Vector3(-20, 0, 90);

                    objetivo.SetPos(objetivo.GetCasillaX(), objetivo.GetCasillaZ());
                    objetivo.transform.eulerAngles = new Vector3(objetivo.transform.eulerAngles.x, objetivo.transform.eulerAngles.y, 0);
                    objetivo.panelHp.SetActive(true);

                    Restaura();
                    if (objetivo.nombre == "Sergio" || objetivo.nombre == "Yaiza")
                    {
                        log.LanzaLog("Está claro que Asier escala más rápido que " + objetivo.nombre + ". Es muy eficaz!!");
                        objetivo.HacerDanyo(dmgAM * bonifDmg * 2);
                    }
                    else
                    {
                        log.LanzaLog("Parece que a Asier se le ha caído el lastre.");
                        objetivo.HacerDanyo(dmgAM * bonifDmg);
                    }
                    musicaFondo.Play();
                    return true;
                }
            }
        }
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if (coche == null)
        {
            PlaySonidoAD();
            coche = Instantiate(cochePrefab);
            coche.transform.position = transform.position;
            if (!aliado)
                coche.transform.Rotate(0, 180, 0);
            transform.position += new Vector3(0, 1000, 0);
        }
        else
        {
            avanzado += Time.deltaTime;
            if(avanzado > 1.5f)
            {
                if (aliado)
                    coche.transform.position += new Vector3(-50 * Time.deltaTime, 0, 0);
                else
                    coche.transform.position += new Vector3(50 * Time.deltaTime, 0, 0);
                if(avanzado>3)
                {
                    FindObjectOfType<Terreno>().ArrasarFilas(casillaX, casillaZ, aliado, dmgAD * bonifDmg);
                    log.LanzaLog("Asier ha atropellado a unas cuantas personas, pero no le han puesto multa.");
                    transform.position -= new Vector3(0, 1000, 0);
                    Restaura();
                    ultimaJugoAD = true;
                    return true;
                }
            }
        }
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {

        BonificacionDamage(2);
        log.LanzaLog("Cuidao que viene Suasenaguer.");
        jugadaUlti = true;
        return true;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        avanzado = 0;
        rotandoDch = true;
        if (coche != null)
            Destroy(coche);
        if (rocodromo != null)
            Destroy(rocodromo);
    }
}
