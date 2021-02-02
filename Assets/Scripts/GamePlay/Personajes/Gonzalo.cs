using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gonzalo : Personaje
{
    public Transform body;
    public GameObject camaPrefab, balonPrefab;
    public AudioSource despertar, golpe;

    GameObject cama, balon;
    Vector3 initialBodyScale;
    Vector3 initialBodyRotation;
    Vector3 guardaPos;
    float avanzado = 0, velBalonX = 0, velBalonZ=0;

    private void Start()
    {
        initialBodyScale = body.localScale;
        initialBodyRotation = body.eulerAngles;
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (body.localScale.x < 4)
        {
            PlaySonidoAM();
            body.localScale += new Vector3(1.3f*Time.deltaTime, 1.0f * Time.deltaTime, 1.5f * Time.deltaTime);
            panelHp.SetActive(false);
            guardaPos = transform.position;
        }
        else
        {
            if(avanzado < 1.2f)
            {
                avanzado += Time.deltaTime;
                if (aliado)
                {
                    transform.position -= new Vector3(0.2f, 0, 0);
                    body.eulerAngles -= new Vector3(10, 0, 0);
                }
                else
                {
                    transform.position += new Vector3(0.2f, 0, 0);
                    body.eulerAngles += new Vector3(10, 0, 0);
                }
                if (avanzado > 0.5f)
                {
                    objetivo.Tumbar();
                }
            }
            else
            {
                Restaura();
                objetivo.RestauraPropiedades();
                log.LanzaLog("Gonzalo usó desenrollar sobre " + objetivo.nombre + ".");
                objetivo.HacerDanyo(dmgAM * bonifDmg);
                return true;
            }
        }
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if (balon == null)
        {
            PlaySonidoAD();
            balon = Instantiate(balonPrefab);
            if(aliado)
                balon.transform.position = transform.position + new Vector3(-0.75f, 0.4f, 0.5f);
            else
                balon.transform.position = transform.position + new Vector3(0.75f, 0.4f, -0.5f);
        }
        else if (avanzado>4)
        {
            if (velBalonX == 0 && velBalonZ == 0)
            {
                FindObjectOfType<Camara>().RestauraCamara();
                velBalonX = objetivo.transform.position.x - balon.transform.position.x;
                velBalonZ = objetivo.transform.position.z - balon.transform.position.z;
            }
            bool llegadoX, llegadoZ;
            if (velBalonX > 0)
                llegadoX = balon.transform.position.x >= objetivo.transform.position.x;
            else
                llegadoX = balon.transform.position.x <= objetivo.transform.position.x;

            if (velBalonZ > 0)
                llegadoZ = balon.transform.position.z >= objetivo.transform.position.z;
            else
                llegadoZ = balon.transform.position.z <= objetivo.transform.position.z;

            if (llegadoX && llegadoZ)
            {
                golpe.Play();
                if (!objetivo.deportista)
                {
                    log.LanzaLog("Gonzalo jugó al matapollos con " + objetivo.nombre + ". Es muy eficaz!!");
                    objetivo.HacerDanyo(dmgAD * bonifDmg * 2);
                }
                else
                {
                    log.LanzaLog("Gonzalo le dio un pase largo a " + objetivo.nombre + ", pero calculó mal.");
                    objetivo.HacerDanyo(dmgAD * bonifDmg);
                }
                velBalonX = 0;
                velBalonZ = 0;
                avanzado = 0;
                Destroy(balon);
                panelHp.SetActive(true);
                ultimaJugoAD = true;
                return true;
            }
            else
            {
                balon.transform.position += new Vector3(velBalonX * Time.deltaTime, 2*Time.deltaTime, velBalonZ * Time.deltaTime);
            }
        }
        avanzado += Time.deltaTime;
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {
        if(cama == null)
        {
            PlaySonidoAE();
            panelHp.SetActive(false);

            IrALaCama();
            
            if (aliado)
                transform.eulerAngles = new Vector3(30, 90, 80);
            else
                transform.eulerAngles = new Vector3(150, 90, 100);
            transform.position += new Vector3(0, 1.5f, 0);
            cama = Instantiate(camaPrefab);
            cama.transform.position = transform.position;
        }
        else if(!sonidoAE.isPlaying)
        {
            jugadaUlti = true;
            turnosInmune = 2;
            turnosParalizado = 2;
            Curar(initialHp);
            log.LanzaLog("La marmota se fue a dormir.");
            return true;
        }
        return false;
    }

    private void Restaura()
    {
        avanzado = 0;
        transform.position = guardaPos;
        transform.eulerAngles = initialRot;
        body.transform.localScale = initialBodyScale;
        body.transform.eulerAngles = initialBodyRotation;
        panelHp.SetActive(true);
    }

    private void IrALaCama()
    {
        Terreno tablero = FindObjectOfType<Terreno>();
        int fila = tablero.GetFilas() - 1;
        if (!aliado)
            fila = 0;
        for(int i = 0; i < tablero.GetColumnas()-1;i++)
        {
            if(!tablero.GetCasilla(fila, i).EstaOcupada() &&
                !tablero.GetCasilla(fila, i+1).EstaOcupada())
            {
                tablero.GetCasilla(casillaX, casillaZ).Desocupar();
                tablero.GetCasilla(fila, i).Ocupar(this);
                tablero.GetCasilla(fila, i+1).Ocupar(this);
                SetPos(fila, i);
                FindObjectOfType<Camara>().EnfocaCamaraAE(transform.position + new Vector3(0, -0.5f, 1.5f), aliado);
                break;
            }
        }
    }
    protected override void RestauraEspecial()
    {
        if (cama != null)
        {
            despertar.Play();
            FindObjectOfType<Terreno>().GetCasilla(casillaX, casillaZ + 1).Desocupar();
            transform.position -= new Vector3(0, 1.5f, 0);
            transform.eulerAngles = initialRot;
            Destroy(cama);
            log.LanzaLog("Parecía que nunca iba a llegar este momento, pero Gonzalo se acaba de despertar.");
        }

        panelHp.SetActive(true);
    }
}
