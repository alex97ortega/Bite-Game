using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yaiza : Personaje
{
    float avanzado = 0;
    public GameObject botellaPrefab, tofuPrefab;
    public Transform manoDch, hombroDch, hombroIzq, codoDch, codoIzq;
    public AudioSource golpe;
    GameObject botella, tofu;
    Vector3 initialHombroDchRot, initialHombroIzqRot, initialCodoDchRot, initialCodoIzqRot;
    bool retirada = false;
    float velPuerroX, velPuerroZ;

    private void Start()
    {
        initialHombroDchRot = hombroDch.eulerAngles;
        initialHombroIzqRot = hombroIzq.eulerAngles;
        initialCodoDchRot = codoDch.eulerAngles;
        initialCodoIzqRot = codoIzq.eulerAngles;
        velPuerroX = 0;
        velPuerroZ = 0;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if(botella == null)
        {
            botella = Instantiate(botellaPrefab);
            botella.transform.position = transform.position;
            if (!aliado)
                botella.transform.Rotate(0, 180, 0);
            botella.transform.parent = manoDch;
            hombroDch.Rotate(140, 0, 20);
            panelHp.SetActive(false);
            objetivo.panelHp.SetActive(false);

            if (aliado)
            {
                transform.position -= new Vector3(1.5f, 0, 0);
                transform.Rotate(0, -20, 0);
            }
            else
            {
                transform.position += new Vector3(1.5f, 0, 0);
            }
        }
        else
        {
            avanzado += Time.deltaTime;

            if (avanzado > 0.5f)
            {
                hombroDch.Rotate(-250 * Time.deltaTime, 0, 0);
                if (avanzado > 1)
                {
                    PlaySonidoAM();
                    Restaura();
                    objetivo.panelHp.SetActive(true);
                    if (aliado)
                    {
                        transform.position += new Vector3(1.5f, 0, 0);
                        transform.Rotate(0, 20, 0);
                    }
                    else
                    {
                        transform.position -= new Vector3(1.5f, 0, 0);
                    }

                    hombroDch.eulerAngles = initialHombroDchRot;
                    if (objetivo.nombre == "Laura" || objetivo.nombre == "Asier")
                    {
                        log.LanzaLog(objetivo.nombre + " decía que quería cerveza, pues toma cerveza. Es muy eficaz!!");
                        objetivo.HacerDanyo(dmgAM * bonifDmg * 2);
                    }
                    else
                    {
                        log.LanzaLog("Ha sonado un poco a hueco.");
                        objetivo.HacerDanyo(dmgAM * bonifDmg);
                    }
                    return true;
                }               
            }
        }
        
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if (tofu == null)
        {
            //PlaySonidoAD();
            tofu = Instantiate(tofuPrefab);
            if (aliado)
            {
                tofu.transform.position = transform.position + new Vector3(0, 2.1f, -1.1f);
                tofu.transform.eulerAngles += new Vector3(15, 0, 0);
            }
            else
            {
                tofu.transform.position = transform.position + new Vector3(0, 2.1f, 1.1f);
                tofu.transform.eulerAngles += new Vector3(-15, 0, 0);
            }
        }
        else if (avanzado>1)
        {
            if (velPuerroX == 0 && velPuerroZ == 0)
            {
                FindObjectOfType<Camara>().RestauraCamara();
                velPuerroX = objetivo.transform.position.x - tofu.transform.position.x;
                velPuerroZ = objetivo.transform.position.z - tofu.transform.position.z;
            }
            bool llegadoX, llegadoZ;
            if (velPuerroX > 0)
                llegadoX = tofu.transform.position.x >= objetivo.transform.position.x;
            else
                llegadoX = tofu.transform.position.x <= objetivo.transform.position.x;

            if (velPuerroZ > 0)
                llegadoZ = tofu.transform.position.z >= objetivo.transform.position.z;
            else
                llegadoZ = tofu.transform.position.z <= objetivo.transform.position.z;

            if (llegadoX && llegadoZ)
            {
                golpe.Play();
                log.LanzaLog("Yaiza le dio tofu del bueno a " + objetivo.nombre + ".");
                objetivo.HacerDanyo(dmgAD * bonifDmg);
                Restaura();
                ultimaJugoAD = true;
                return true;
            }
            else
            {
                tofu.transform.position += new Vector3(velPuerroX * Time.deltaTime, 0, velPuerroZ * Time.deltaTime);
                tofu.transform.Rotate(-500 * Time.deltaTime, 0, -500 * Time.deltaTime);
            }
        }
        avanzado += Time.deltaTime;
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {
        if(!retirada)
        {
            retirada = true;
            //PlaySonidoAE();
            panelHp.SetActive(false);
            if (aliado)
                transform.position = new Vector3(35, 0, 9);
            else
                transform.position = new Vector3(-9, 0, 9);
            hombroDch.Rotate(0, -70, 10);
            hombroIzq.Rotate(0, 70, -10);
            codoDch.Rotate(95, 0, 15);
            codoIzq.Rotate(110, 0, -20);

            FindObjectOfType<Camara>().EnfocaCamaraAE(transform.position, aliado);
            Terreno tablero = FindObjectOfType<Terreno>();
            tablero.GetCasilla(casillaX, casillaZ).Desocupar();
        }
        else
        {
            avanzado += Time.deltaTime;
            if(avanzado > 2)
            {
                Restaura();
                log.LanzaLog("Yaiza se ha cansado y no quiere jugar más.");
                turnosInmune = 2;
                turnosParalizado = 2;
                jugadaUlti = true;
                if(aliado)
                {
                    foreach (var p in FindObjectOfType<GestorPartida>().GetAllAliados())
                        p.BonificacionDamage(2);
                }
                else
                {
                    foreach (var p in FindObjectOfType<GestorPartida>().GetAllEnemigos())
                        p.BonificacionDamage(2);
                }
                return true;
            }
        }
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        avanzado = 0;
        velPuerroX = 0;
        velPuerroZ = 0;
        if (botella)
            Destroy(botella);
        if (tofu)
            Destroy(tofu);
    }

    protected override void RestauraEspecial()
    {
        if (retirada)
        {
            Terreno tablero = FindObjectOfType<Terreno>();
            int fila = tablero.GetFilas() - 1;
            if (!aliado)
                fila = 0;
            for (int i = 0; i < tablero.GetColumnas() - 1; i++)
            {
                if (!tablero.GetCasilla(fila, i).EstaOcupada())
                {
                    tablero.GetCasilla(casillaX, casillaZ).Desocupar();
                    tablero.GetCasilla(fila, i).Ocupar(this);
                    SetPos(fila, i);
                    break;
                }
            }
            log.LanzaLog("A Yaiza se le ha pasado el cabreo por fin.");
            retirada = false;
            hombroDch.eulerAngles = initialHombroDchRot;
            hombroIzq.eulerAngles = initialHombroIzqRot;
            codoDch.eulerAngles = initialCodoDchRot;
            codoIzq.eulerAngles = initialCodoIzqRot;
        }
    }
}
