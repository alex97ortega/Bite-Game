using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    private bool ocupada = false;
    private Vector3 cas;
    
    public bool EstaOcupada() { return ocupada; }
    public void Ocupar() { ocupada = true; }
    public void Desocupar() { ocupada = false; }

    public void SetPos(int x, int y, int z)
    {
        cas = new Vector3(x, y, z);
        transform.position = new Vector3(x*3, y, z*3);
    }
    public Vector3 GetCas() { return cas; }
}
