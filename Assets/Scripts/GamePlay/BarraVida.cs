using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraVida : MonoBehaviour
{
    public Material verde, amarillo, rojo;

    public void CambiaVerde() { GetComponent<MeshRenderer>().material = verde; }
    public void CambiaAmarillo() { GetComponent<MeshRenderer>().material = amarillo; }
    public void CambiaRojo() { GetComponent<MeshRenderer>().material = rojo; }
}
