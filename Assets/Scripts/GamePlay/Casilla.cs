using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    private bool ocupada;
    // Start is called before the first frame update
    void Start()
    {
        ocupada = false;
    }
    
    public bool EstaOcupada() { return ocupada; }
    public void Ocupar() { ocupada = true; }
    public void Desocupar() { ocupada = false; }

    public void SetPos(int x, int y, int z)
    {
        transform.position = new Vector3(x, y, z);
    }
}
