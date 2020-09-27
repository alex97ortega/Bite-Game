using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microondas : MonoBehaviour
{
    public MeshRenderer pantalla;
    float contColor = 0;
    bool baja = true;

    // Update is called once per frame
    void Update()
    {
        if(baja)
        {
            contColor += 3*Time.deltaTime;
            pantalla.material.color = new Color(contColor, contColor, 0);
            if (contColor > 0.8f)
            {
                contColor = 0;
                baja = false;
            }
        }
        else
        {
            contColor += 3*Time.deltaTime;
            pantalla.material.color = new Color(0.8f-contColor, 0.8f - contColor, 0);
            if (contColor > 1)
            {
                contColor = 0;
                baja = true;
            }
        }
    }
}
