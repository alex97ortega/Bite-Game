using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gonzalo : Personaje
{
    public Transform body;
    public GameObject camaPrefab;
    GameObject cama;
    Vector3 initialBodyScale;
    Vector3 initialBodyRotation;
    Vector3 guardaPos;
    float avanzado = 0;

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
            body.localScale += new Vector3(1.5f*Time.deltaTime, 1.1f * Time.deltaTime, 1.75f * Time.deltaTime);
            panelHp.SetActive(false);
            guardaPos = transform.position;
        }
        else
        {
            if(aliado)
            {
                if(avanzado < 1.2f)
                {
                    avanzado += Time.deltaTime;
                    transform.position -= new Vector3(0.2f, 0, 0);
                    body.eulerAngles -= new Vector3(10, 0, 0);
                    if (avanzado > 0.5f)
                    {
                        objetivo.Tumbar();
                    }
                }
                else
                {
                    Restaura();
                    objetivo.RestauraPropiedades();
                    objetivo.HacerDanyo(dmgAM);
                    return true;
                }
            }
            //enemigo
            else
            {
                if (avanzado < 1.2f)
                {
                    avanzado += Time.deltaTime;
                    transform.position += new Vector3(0.2f, 0, 0);
                    body.eulerAngles += new Vector3(10, 0, 0);
                    if(avanzado > 0.5f)
                    {
                        objetivo.Tumbar();
                    }
                }
                else
                {
                    Restaura();
                    objetivo.RestauraPropiedades();
                    objetivo.HacerDanyo(dmgAM);
                    return true;
                }
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
            turnosInmune = 3;
            turnosParalizado = 3;
            Curar(initialHp);
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
                FindObjectOfType<Camara>().EnfocaCamaraAE(transform.position + new Vector3(0, -0.5f, 2), aliado);
                break;
            }
        }
    }
    protected override void RestauraEspecial()
    {
        if (cama != null)
        {
            FindObjectOfType<Terreno>().GetCasilla(casillaX, casillaZ + 1).Desocupar();
            transform.position -= new Vector3(0, 1.5f, 0);
            transform.eulerAngles = initialRot;
            Destroy(cama);
        }

        panelHp.SetActive(true);
    }
}
