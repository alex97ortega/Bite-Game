using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    int casillaX, casillaZ;
    public int id;
    public string nombre;
    public Sprite foto;
    public bool chico;
    public bool deportista;
    public bool gaymer;
    public bool fumador;
    public int movimientos;
    public int hp;
    public int dmgAM, dmgAD, dmgAE;

    public string infoAC, infoAD, infoAE;

    public GameObject panelHp;
    public GameObject barraVerdeHp;
    public AudioSource sonidoAM, sonidoAD, sonidoAE;

    protected int initialHp;
    protected bool aliado = true;
    protected bool muerto = false;
    protected bool jugadaUlti = false;
    protected Vector3 initialRot, initialScale;

    void Awake()
    {
        initialHp = hp;
        initialRot = transform.eulerAngles;
        initialScale = transform.localScale;
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
        if(!sonidoAM.isPlaying)
            sonidoAM.Play();
    }
    public void PlaySonidoAD()
    {
        if (!sonidoAD.isPlaying)
            sonidoAD.Play();
    }
    public void PlaySonidoAE()
    {
        if (!sonidoAE.isPlaying)
            sonidoAE.Play();
    }

    public void HacerDanyo(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            muerto = true;
            Tumbar();
        }

        float relation = (float)hp / (float)initialHp;
        barraVerdeHp.transform.localScale = new Vector3(1, 1, relation);
        if (relation < 0.3f)
            barraVerdeHp.GetComponentInChildren<BarraVida>().CambiaRojo();
        else if (relation < 0.6f)
            barraVerdeHp.GetComponentInChildren<BarraVida>().CambiaAmarillo();
    }

    public bool EstaMuerto() { return muerto; }
    public bool HaJugadoUlti() { return jugadaUlti; }
}
