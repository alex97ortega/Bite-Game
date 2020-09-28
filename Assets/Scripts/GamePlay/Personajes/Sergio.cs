using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sergio : Personaje
{
    public Ropa ropaPrefab;
    Vector3 initialTorsoRot;
    List<Ropa> ropaRegalada;
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
                ;// PlaySonidoAM();

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
                objetivo.HacerDanyo(dmgAM);
                return true;
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
    }
}
