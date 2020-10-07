﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public int id;
    public string nombre;
    public Sprite foto;
    public bool chico;
    public bool deportista;
    public bool gaymer;
    public bool fumador;
    public bool necesitaObjetivoAD;
    public int movimientos;
    public int hp;
    public int dmgAM, dmgAD, dmgAE;

    public string infoAC, infoAD, infoAE;

    public GameObject panelHp;
    public GameObject barraVerdeHp;
    public AudioSource sonidoAM, sonidoAD, sonidoAE, cancion;

    protected int casillaX, casillaZ;
    protected int initialHp, initialMovs;
    protected Vector3 initialRot, initialScale;
    protected Log log;

    protected bool aliado = true;
    protected bool muerto = false;
    protected bool jugadaUlti = false;
    protected bool ultimaJugoAD = false;
    protected int turnosParalizado = 0;
    protected int turnosInmune = 0;
    protected int turnosBonifVelocidad = 0;
    protected Stack<int> turnosDmgx2;
    protected int bonifDmg = 1;

    void Awake()
    {
        initialHp = hp;
        initialMovs = movimientos;
        initialRot = transform.eulerAngles;
        initialScale = transform.localScale;
        log = FindObjectOfType<Log>();
        turnosDmgx2 = new Stack<int>();
    }

    public void SetPos(int x, int z)
    {
        casillaX = x;
        casillaZ = z;
        transform.position = new Vector3(x * 3, 0, z * 3);
    }
    public void Girar()
    {
        transform.Rotate(0, 180, 0);
        panelHp.transform.Rotate(180, 0, 0);
        initialRot = transform.eulerAngles;
    }
    public void SetColor(Material color)
    {
        foreach (var i in GetComponentsInChildren<MeshRenderer>())
        {
            if(i.gameObject.tag != "Cabeza" && i.gameObject.tag != "Unia" && i.gameObject.tag != "Hp")
                i.material = color;
        }
    }
    public Sprite GetFoto() { return foto; }
    public int GetCasillaX() { return casillaX; }
    public int GetCasillaZ() { return casillaZ; }
    public void SetEnemigo() { aliado = false; }
    public bool IsAliado() { return aliado; }

    public virtual bool AnimacionAM(Personaje objetivo) { return false; }
    public virtual bool AnimacionAD(Personaje objetivo) { return false; }
    public virtual bool AnimacionAE(Personaje objetivo) { return false; }
    protected virtual void RestauraEspecial() { }

    public void RestauraPropiedades()
    {
        panelHp.SetActive(true);
        transform.position = new Vector3(casillaX * 3, 0, casillaZ * 3);
        transform.eulerAngles = initialRot;
        transform.localScale = initialScale;
    }

    public void Tumbar()
    {
        panelHp.SetActive(false);
        transform.position = new Vector3(casillaX * 3, 0, casillaZ * 3) + new Vector3(1.5f, 0.2f, 0);
        transform.eulerAngles = initialRot + new Vector3(90, 0, 0);
    }

    public void PlaySonidoAM()
    {
        if(sonidoAM != null && !sonidoAM.isPlaying)
            sonidoAM.Play();
    }
    public void PlaySonidoAD()
    {
        if (sonidoAD != null && !sonidoAD.isPlaying)
            sonidoAD.Play();
    }
    public void PlaySonidoAE()
    {
        if (sonidoAE != null && !sonidoAE.isPlaying)
            sonidoAE.Play();
    }
    public void PlayCancion()
    {
        if (cancion != null && !cancion.isPlaying)
            cancion.Play();
    }
    public bool CancionFinished()
    {
        return !cancion.isPlaying;
    }


    public void HacerDanyo(int dmg)
    {
        if (muerto || turnosInmune > 0)
            return;

        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            muerto = true;
            Tumbar();
            int rnd = Random.Range(0, 3);
            if (rnd == 0)
                log.LanzaLog("Baia, " + nombre + " se ha quedao tieso.");
            else if (rnd == 1)
                log.LanzaLog(nombre + " no ha podido aguantar ese fatal ataque.");
            else
                log.LanzaLog("Parece que " + nombre + " la acaba de palmar.");
        }

        float relation = (float)hp / (float)initialHp;
        barraVerdeHp.transform.localScale = new Vector3(1, 1, relation);
        if (relation < 0.3f)
            barraVerdeHp.GetComponentInChildren<BarraVida>().CambiaRojo();
        else if (relation < 0.6f)
            barraVerdeHp.GetComponentInChildren<BarraVida>().CambiaAmarillo();
    }

    public void Curar(int cantidad)
    {
        if (muerto)
            return;

        hp += cantidad;
        if (hp > initialHp)
            hp = initialHp;

        float relation = (float)hp / (float)initialHp;
        barraVerdeHp.transform.localScale = new Vector3(1, 1, relation);

        if (relation >= 0.6f)
            barraVerdeHp.GetComponentInChildren<BarraVida>().CambiaVerde();
        else if (relation >= 0.3f)
            barraVerdeHp.GetComponentInChildren<BarraVida>().CambiaAmarillo();
    }

    public bool EstaMuerto() { return muerto; }
    public bool HaJugadoUlti() { return jugadaUlti; }
    public bool EsInmune() { return turnosInmune > 0; }

    // en esta llamada aprovechamos y ya gestionamos todo al empezar el turno
    public bool EstaParalizado()
    {
        ComienzoTurno();

        if (turnosParalizado == 0)
        {
            RestauraEspecial();
            return false;
        }

        turnosParalizado--;
        return true;
    }

    private void ComienzoTurno()
    {
        if (turnosInmune != 0)
            turnosInmune--;
        if (turnosBonifVelocidad != 0)
            turnosBonifVelocidad--;
        else
            movimientos = initialMovs;

        if(turnosDmgx2.Count != 0)
        {
            int aux = turnosDmgx2.Pop();
            aux--;
            if (aux < 0)
            {
                bonifDmg /= 2;
                // quitamos si hubiera algun 0
                while(turnosDmgx2.Count != 0 && turnosDmgx2.Peek()==0)
                {
                    turnosDmgx2.Pop();
                    bonifDmg /= 2;
                }
            }
            else
                turnosDmgx2.Push(aux);
        }
    }

    protected void BonificacionDamage(int turnos)
    {
        bonifDmg *= 2;

        Queue<int> cola = new Queue<int>();
        int turnosActualesDmgx2 = 0;

        while(turnosDmgx2.Count != 0)
        {
            int aux = turnosDmgx2.Pop();
            turnosActualesDmgx2 += aux;
            cola.Enqueue(aux);
        }
        turnos -= turnosActualesDmgx2;
        turnosDmgx2.Push(turnos);

        while(cola.Count!=0)
        {
            int aux = cola.Dequeue();
            turnosDmgx2.Push(aux);
        }
    }

    public bool UltimaJugoAtaqueDistancia()
    {
        if(ultimaJugoAD)
        {
            ultimaJugoAD = false;
            return true;
        }
        return false;
    }
}
