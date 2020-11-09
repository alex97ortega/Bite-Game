using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asier : Personaje
{
    public GameObject paredPrefab, cochePrefab, barraPrefab;
    public Transform hombroIzq, hombroDch, manoDch, rodillas;
    public AudioSource golpe;
    
    float avanzado = 0;
    int dominadas = 0;
    bool rotandoDch = true, subiendo = true;
    GameObject rocodromo, coche, barra;
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
        if(barra == null)
        {
            PlaySonidoAE();
            barra = Instantiate(barraPrefab);
            barra.transform.position = transform.position;
            if (!aliado)
            {
                barra.transform.Rotate(0, 180, 0);
                if (FindObjectOfType<GameManager>().IsAliado())
                    barra.GetComponent<Barras>().SetColorRojo();
            }
            else
            {
                if (!FindObjectOfType<GameManager>().IsAliado())
                    barra.GetComponent<Barras>().SetColorAzul();
            }

            panelHp.SetActive(false);
            hombroDch.gameObject.SetActive(false);
            hombroIzq.gameObject.SetActive(false);
            rodillas.eulerAngles -= new Vector3(90, 0, 0);
        }
        else
        {
            if(subiendo)
            {
                avanzado += Time.deltaTime;
                transform.position += new Vector3(0, 1.5f*Time.deltaTime, 0);
                if(avanzado >= 0.4f)
                {
                    subiendo = false;
                    dominadas++;
                    avanzado = 0;
                }
            }
            else
            {
                avanzado += Time.deltaTime;
                transform.position -= new Vector3(0, 1.5f * Time.deltaTime, 0);
                if (avanzado >= 0.4f)
                {
                    subiendo = true;
                    avanzado = 0;
                }
            }

            if(!sonidoAE.isPlaying)
            {
                BonificacionDamage(2);
                log.LanzaLog("Cuidao que viene Suasenaguer.");
                jugadaUlti = true;
                hombroDch.gameObject.SetActive(true);
                hombroIzq.gameObject.SetActive(true);
                rodillas.eulerAngles += new Vector3(90, 0, 0);
                SetPos(casillaX, casillaZ);
                Restaura();
                return true;
            }
        }
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        avanzado = 0;
        dominadas = 0;
        rotandoDch = true;
        subiendo = true;
        if (coche != null)
            Destroy(coche);
        if (rocodromo != null)
            Destroy(rocodromo);
        if (barra != null)
            Destroy(barra);
    }
}
