using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    private bool ocupada = false;
    private bool envenenada = false;
    private Vector3 cas;
    private Personaje personajeCasilla;
    private int veneno = 0;
    
    public bool EstaOcupada() { return ocupada; }

    public void Ocupar(Personaje p)
    {
        ocupada = true;
        personajeCasilla = p;
    }

    public void Desocupar()
    {
        ocupada = false;
        personajeCasilla = null;
    }

    public void SetPos(int x, int y, int z)
    {
        cas = new Vector3(x, y, z);
        transform.position = new Vector3(x*3, y, z*3);
    }
    public Vector3 GetCas() { return cas; }
    public Personaje GetPersonajeCasilla() { return personajeCasilla; }

    public bool EstaEnvenenada() { return envenenada; }
    public int GetVenenoDmg() { return veneno; }
    public void Envenenar(int dmg)
    {
        envenenada = true;
        veneno += dmg;
    }
}
