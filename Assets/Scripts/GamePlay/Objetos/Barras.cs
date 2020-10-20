using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barras : MonoBehaviour
{
    public MeshRenderer[] objetosAmarillos;
    public Transform codos;
    public Material rojo;

    float avanzado = 0;
    bool subiendo = true;

    public void SetColorRojo()
    {
        foreach (var o in objetosAmarillos)
            o.material = rojo;
    }

    // Update is called once per frame
    void Update()
    {
        if (subiendo)
        {
            avanzado += Time.deltaTime;
            codos.eulerAngles -= new Vector3(100*Time.deltaTime, 0, 0);
            if (avanzado >= 0.4f)
            {
                subiendo = false;
                avanzado = 0;
            }
        }
        else
        {
            avanzado += Time.deltaTime;
            codos.eulerAngles += new Vector3(100*Time.deltaTime, 0, 0);
            if (avanzado >= 0.4f)
            {
                subiendo = true;
                avanzado = 0;
            }
        }
    }
}
