using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alex : Personaje
{
    public GameObject palomitasPrefab, microondasPrefab, apioPrefab;
    public AudioSource sonidoMicro;
    float avanzado = 0;
    GameObject palomitas, microondas, apio;
    
    public override bool AnimacionAM(Personaje objetivo)
    {
        if (avanzado < 1)
        {
            panelHp.SetActive(false);
            PlaySonidoAM();
            avanzado += Time.deltaTime;
        }
        else
        {

            if (palomitas==null)
            {
                palomitas = Instantiate(palomitasPrefab);
                palomitas.transform.position = objetivo.transform.position + new Vector3(0, 1.5f ,0);
                if (!aliado)
                    palomitas.transform.eulerAngles = new Vector3(0, -50, 0);
                objetivo.gameObject.SetActive(false);
                avanzado = 0.5f;
            }
            else if(palomitas.transform.localScale.y > 1)
            {
                palomitas.transform.localScale -= new Vector3(0, 3*Time.deltaTime, 3 * Time.deltaTime);
                palomitas.transform.position -= new Vector3(0, 1.5f * Time.deltaTime, 0);
            }
            else if(microondas == null)
            {
                microondas = Instantiate(microondasPrefab);
                microondas.transform.position = objetivo.transform.position + new Vector3(0, 1, 0);
                if (!aliado)
                    microondas.transform.eulerAngles = new Vector3(0, -240, 0);
                sonidoMicro.Play();
            }
            else
            {
                avanzado+= Time.deltaTime;
                if(avanzado>3.5f)
                {
                    objetivo.gameObject.SetActive(true);
                    objetivo.HacerDanyo(dmgAM);
                    Restaura();
                    return true;
                }
            }
        }
        return false;
    }

    public override bool AnimacionAD(Personaje objetivo)
    {
        if(apio== null)
        {
            apio = Instantiate(apioPrefab);
            if(aliado)
            {
                apio.transform.position = transform.position + new Vector3(0,2.1f, -1.1f);
                apio.transform.eulerAngles += new Vector3(15, 0, 0);
            }
            else
            {
                apio.transform.position = transform.position + new Vector3(0, 2.1f, 1.1f);
                apio.transform.eulerAngles += new Vector3(-15, 0, 0);
            }
        }
        return false;
    }

    public override bool AnimacionAE(Personaje objetivo)
    {
        return false;
    }

    private void Restaura()
    {
        panelHp.SetActive(true);
        if (palomitas != null)
            Destroy(palomitas);
        if (microondas != null)
            Destroy(microondas);

        avanzado = 0;
    }
}
