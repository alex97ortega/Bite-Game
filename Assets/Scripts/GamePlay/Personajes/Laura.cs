using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laura : Personaje
{
    public Transform torso, hombroIzq, hombroDch;
    public GameObject burgerPrefab;
    public AudioSource golpe;

    GameObject burger;
    Vector3 iniRotIzq, iniRotDch;
    Vector3 iniRotTorso;

    int veces = 0;
    float movido = 60, avanzado = 0, velBurgerX = 0, velBurgerZ = 0;
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
        if (burger == null)
        {
            PlaySonidoAD();
            burger = Instantiate(burgerPrefab);
            hombroDch.transform.eulerAngles -= new Vector3(25, 0, 70);
            if (aliado)
            {
                burger.transform.position = transform.position + new Vector3(-0.1f, 2.7f, 0.85f);
            }
            else
            {
                burger.transform.position = transform.position + new Vector3(0.1f, 2.7f, -0.85f);
            }
        }
        else if (avanzado >= 1.0f)
        {
            if (velBurgerX == 0 && velBurgerZ == 0)
            {
                FindObjectOfType<Camara>().RestauraCamara();
                velBurgerX = objetivo.transform.position.x - burger.transform.position.x;
                velBurgerZ = objetivo.transform.position.z - burger.transform.position.z;
            }
            bool llegadoX, llegadoZ;
            if (velBurgerX > 0)
                llegadoX = burger.transform.position.x >= objetivo.transform.position.x;
            else
                llegadoX = burger.transform.position.x <= objetivo.transform.position.x;

            if (velBurgerZ > 0)
                llegadoZ = burger.transform.position.z >= objetivo.transform.position.z;
            else
                llegadoZ = burger.transform.position.z <= objetivo.transform.position.z;

            if (llegadoX && llegadoZ)
            {
                golpe.Play();
                log.LanzaLog("Parece que a " + objetivo.nombre + " no le ha gustado la hamburguesa del TFG.");
                objetivo.HacerDanyo(dmgAD * bonifDmg);
                avanzado = 0;
                velBurgerX = 0;
                velBurgerZ = 0;
                Destroy(burger);
                hombroDch.transform.eulerAngles += new Vector3(25, 0, 70);
                ultimaJugoAD = true;
                return true;
            }
            else
            {
                burger.transform.position += new Vector3(velBurgerX * 1.5f * Time.deltaTime, 0, velBurgerZ * 1.5f * Time.deltaTime);
                burger.transform.Rotate(-50 * Time.deltaTime, 0, -50 * Time.deltaTime);
            }
        }
        avanzado += Time.deltaTime;
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
