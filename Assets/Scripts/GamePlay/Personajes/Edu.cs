using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edu : Personaje
{
    public GameObject palaPrefab, carritoPrefab;
    public Transform manoDch, hombroDch, codoDch, piernaDch, piernaIzq;
    public AudioSource golpe;
    public ParticleSystem sangrePrefab;

    Vector3 initialHombroDchRot, guardaPosCabesa, initalBodyScale, initialPiernaDchRot, initialPiernaIzqRot;
    GameObject pala, carrito;
    ParticleSystem sangre;
    float avanzado = 0;
    float avanzadoCamin = 15;
    float velCarritoX, velCarritoZ;
    bool flag = false;
    bool flag2 = false;
    bool caminandoIzq = true;

    private void Start()
    {
        initialHombroDchRot = hombroDch.eulerAngles;
        initalBodyScale = transform.localScale;
        initialPiernaDchRot = piernaDch.eulerAngles;
        initialPiernaIzqRot = piernaIzq.eulerAngles;
        velCarritoX = 0;
        velCarritoZ = 0;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (pala == null)
        {
            PlaySonidoAM();
            pala = Instantiate(palaPrefab);
            pala.transform.position = transform.position;
            if (!aliado)
                pala.transform.Rotate(0, 180, 0);
            codoDch.Rotate(0, 190, 0);
            pala.transform.parent = manoDch.transform;
            if (aliado)
            {
                transform.position -= new Vector3(1.5f, 0, 0);
                transform.Rotate(0, -20, 0);
            }
            else
                transform.position += new Vector3(1.5f, 0, 0);
            hombroDch.Rotate(0, 0, -45);
            panelHp.SetActive(false);
            objetivo.panelHp.SetActive(false);
            guardaPosCabesa = objetivo.cabesa.position;

        }
        else 
        {
            avanzado += Time.deltaTime;
            if (avanzado > 1)
            {
                if (avanzado < 1.3f)
                    hombroDch.eulerAngles += new Vector3(0, -500 * Time.deltaTime, 0);
                else
                {
                    if (avanzado > 3)
                    {
                        Restaura();
                        objetivo.cabesa.position = guardaPosCabesa;
                        objetivo.panelHp.SetActive(true);
                        if (aliado)
                        {
                            transform.position += new Vector3(1.5f, 0, 0);
                            transform.Rotate(0, 20, 0);
                        }
                        else
                            transform.position -= new Vector3(1.5f, 0, 0);
                        codoDch.Rotate(0, -190, 0);
                        hombroDch.eulerAngles = initialHombroDchRot;

                        if (objetivo.nombre == "Dani")
                        {
                            log.LanzaLog("NASA: Localizado meteorito en órbita terrestre.");
                        }
                        else
                        {
                            log.LanzaLog("Home Run!!");
                        }
                        objetivo.HacerDanyo(dmgAM * bonifDmg);
                        return true;
                    }
                    else
                    {
                        if(!flag)
                        {
                            golpe.Play();
                            flag = true;
                        }
                        if(aliado)
                            objetivo.cabesa.position += new Vector3(-25 * Time.deltaTime, 2 * Time.deltaTime, -25 * Time.deltaTime);
                        else
                            objetivo.cabesa.position += new Vector3(25 * Time.deltaTime, 2 * Time.deltaTime, 25 * Time.deltaTime);
                    }
                }
            }               
        }
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if (carrito == null)
        {
            carrito = Instantiate(carritoPrefab);
            carrito.transform.position = transform.position;
            if (aliado)
                carrito.transform.position -= new Vector3(1.4f, 0,0);
            else
            {
                carrito.transform.Rotate(0, 180, 0);
                carrito.transform.position += new Vector3(1.4f, 0, 0);
            }
        }
        else if (avanzado >= 1.0f)
        {
            if (!sonidoAD.isPlaying)
                PlaySonidoAD();
            if (velCarritoX == 0 && velCarritoZ == 0)
            {
                FindObjectOfType<Camara>().RestauraCamara();
                velCarritoX = objetivo.transform.position.x - carrito.transform.position.x;
                velCarritoZ = objetivo.transform.position.z - carrito.transform.position.z;
            }
            bool llegadoX, llegadoZ;
            if (velCarritoX > 0)
                llegadoX = carrito.transform.position.x >= objetivo.transform.position.x;
            else
                llegadoX = carrito.transform.position.x <= objetivo.transform.position.x;

            if (velCarritoZ > 0)
                llegadoZ = carrito.transform.position.z >= objetivo.transform.position.z;
            else
                llegadoZ = carrito.transform.position.z <= objetivo.transform.position.z;

            if (llegadoX && llegadoZ)
            {
                Restaura();
                if (objetivo.nombre == "Gonzalo" || objetivo.nombre == "Victor")
                {
                    log.LanzaLog(objetivo.nombre + " prefiere los carritos del Mercadona. Es muy eficaz!!");
                    objetivo.HacerDanyo(dmgAD * bonifDmg * 2);
                }
                else
                {
                    log.LanzaLog(objetivo.nombre + " se ha ido a hacer la compra.");
                    objetivo.HacerDanyo(dmgAD * bonifDmg);
                }
                ultimaJugoAD = true;
                return true;
            }
            else
            {
                carrito.transform.position += new Vector3(velCarritoX * Time.deltaTime, 0, velCarritoZ * Time.deltaTime);
            }
        }
        avanzado += Time.deltaTime;
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {
        if(avanzado < 0.8f)
        {
            if (!sonidoAE.isPlaying)
            {
                PlaySonidoAE();
                panelHp.SetActive(false);
                objetivo.panelHp.SetActive(false);
            }
            transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
        }
        else
        {
            if(avanzado < 3.9f)
            {
                MuevePiernas();
                if (aliado)
                    transform.position -= new Vector3(Time.deltaTime, 0, 0);
                else
                    transform.position += new Vector3(Time.deltaTime, 0, 0);
            }
            else
            {
                if(avanzado < 5)
                {
                    if (!flag)
                    {
                        flag = true;
                        transform.Rotate(0, 180, 0);
                    }
                    transform.position += new Vector3(0, Time.deltaTime * 1.3f, 0);
                }
                else
                {
                    
                    if(avanzado > 6)
                    {
                        if (avanzado < 6.4f)
                        {
                            if (!flag2)
                            {
                                flag2 = true;
                                sangre = Instantiate(sangrePrefab);
                                sangre.gameObject.transform.position = transform.position;
                                objetivo.transform.position += new Vector3(0, 1000, 0);
                                if (aliado)
                                    transform.Rotate(0, 90, 0);
                                else
                                    transform.Rotate(0, -90, 0);
                            }
                            transform.localScale += new Vector3(Time.deltaTime * 2, Time.deltaTime * 2, Time.deltaTime * 2);
                            transform.position -= new Vector3(0, Time.deltaTime * 3.2f, 0);
                        }
                        else
                        {
                            if(avanzado > 7.8f)
                            {
                                Restaura();
                                if (aliado)
                                    transform.Rotate(0, 90, 0);
                                else
                                    transform.Rotate(0, -90, 0);
                                transform.localScale = initalBodyScale;
                                SetPos(casillaX, casillaZ);
                                objetivo.transform.position -= new Vector3(0, 1000, 0);
                                objetivo.HacerDanyo(1000);
                                jugadaUlti = true;
                                return true;
                            }
                        }

                    }
                }
            }
        }
        avanzado += Time.deltaTime;
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        avanzado = 0;
        avanzadoCamin = 15;
        velCarritoX = 0;
        velCarritoZ = 0;
        flag = false;
        flag2 = false;
        caminandoIzq = true;
        if (pala)
            Destroy(pala);
        if (carrito)
            Destroy(carrito);
        if (sangre)
            Destroy(sangre);
    }

    void MuevePiernas()
    {
        if (caminandoIzq)
        {
            avanzadoCamin += 60 * Time.deltaTime;
            piernaIzq.eulerAngles -= new Vector3(60 * Time.deltaTime, 0, 0);
            piernaDch.eulerAngles += new Vector3(60 * Time.deltaTime, 0, 0);
            if (avanzadoCamin >= 30)
            {
                avanzadoCamin = 0;
                caminandoIzq = false;
            }
        }
        else
        {
            avanzadoCamin += 60 * Time.deltaTime;
            piernaDch.eulerAngles -= new Vector3(60 * Time.deltaTime, 0, 0);
            piernaIzq.eulerAngles += new Vector3(60 * Time.deltaTime, 0, 0);
            if (avanzadoCamin >= 30)
            {
                avanzadoCamin = 0;
                caminandoIzq = true;
            }
        }
    }
}
