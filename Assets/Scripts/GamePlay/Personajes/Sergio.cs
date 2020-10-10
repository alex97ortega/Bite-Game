using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sergio : Personaje
{
    public Ropa ropaPrefab;
    public GameObject cajaPizzaPrefab, cartasMagicPrefab;
    public Transform hombroIzq, hombroDch;

    GameObject cartasMagic;
    Vector3 initialTorsoRot;
    List<Ropa> ropaRegalada;
    GameObject cajaPizza;
    float avanzado = 0;
    float acum = 0;

    private void Start()
    {
        ropaRegalada = new List<Ropa>();
    }

    public override bool AnimacionAM(Personaje objetivo)
    {
        if (ropaRegalada.Count < 30)
        {
            if (ropaRegalada.Count == 0)
                PlaySonidoAM();

            avanzado += Time.deltaTime;
            if(avanzado > 0.15f)
            {
                avanzado = 0;
                Ropa newRopa = Instantiate(ropaPrefab);
                newRopa.transform.position = objetivo.transform.position + new Vector3(Random.Range(-1.0f,1.0f), 3, Random.Range(-1.3f, 0));
                acum += 0.08f;
                newRopa.AddY(acum);
                ropaRegalada.Add(newRopa);
            }
        }
        else
        {
            avanzado += Time.deltaTime;
            if (avanzado > 0.5f)
            {
                Restaura();
                log.LanzaLog("Sergio regaló toda su ropa a " + objetivo.nombre + " y le terminó aplastando.");
                objetivo.HacerDanyo(dmgAM * bonifDmg);
                return true;
            }
        }
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if (cartasMagic == null)
        {
            PlaySonidoAD();
            hombroDch.transform.eulerAngles += new Vector3(52, 0, 54);
            hombroIzq.transform.eulerAngles += new Vector3(52, 0, -54);
            cartasMagic = Instantiate(cartasMagicPrefab);
            cartasMagic.transform.position = transform.position;
            if (!aliado)
                cartasMagic.transform.Rotate(0, 180, 0);
            cartasMagic.GetComponent<CartasMagic>().SetObjetivo(objetivo.transform.position.x, objetivo.transform.position.z);
        }
        else if (!sonidoAD.isPlaying)
        {
            FindObjectOfType<Camara>().RestauraCamara();
            if (cartasMagic.GetComponent<CartasMagic>().LanzadasTodasLasCartas())
            {
                if (objetivo.nombre == "Alex" || objetivo.nombre == "Dani")
                {
                    log.LanzaLog(objetivo.nombre + " perdió dolorosamente a las Magic contra Sergio. Es muy eficaz!!");
                    objetivo.HacerDanyo(dmgAD * bonifDmg * 2);
                }
                else
                {
                    log.LanzaLog("Sergio se marcó un dinamó sobre " + objetivo.nombre + ".");
                    objetivo.HacerDanyo(dmgAD * bonifDmg);
                }
                ultimaJugoAD = true;
                hombroDch.transform.eulerAngles -= new Vector3(52, 0, 54);
                hombroIzq.transform.eulerAngles -= new Vector3(52, 0, -54);
                Destroy(cartasMagic);
                return true;
            }
        }
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {
        if (cajaPizza == null)
        {
            PlaySonidoAE();
            cajaPizza = Instantiate(cajaPizzaPrefab);
            cajaPizza.transform.position = transform.position;
            hombroDch.eulerAngles += new Vector3(0, -80, 0);
            hombroIzq.eulerAngles += new Vector3(0, 80, 0);
            
            if(!aliado)
            {
                cajaPizza.transform.eulerAngles += new Vector3(0, 180, 0);
                cajaPizza.GetComponent<CajaPizza>().SetEnemigo();
            }
        }
        else if(cajaPizza.GetComponent<CajaPizza>().Finished())
        {
            hombroDch.eulerAngles -= new Vector3(0, -80, 0);
            hombroIzq.eulerAngles -= new Vector3(0, 80, 0);
            Restaura();
            Curar(initialHp);
            jugadaUlti = true;
            log.LanzaLog("Sergio nunca desaprovecha los bordes de pizza del día anterior.");
            return true;
        }
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        avanzado = 0;
        acum = 0;
        foreach (var r in ropaRegalada)
            Destroy(r.gameObject);
        ropaRegalada.Clear();
        if (cajaPizza != null)
            Destroy(cajaPizza);
    }
}
