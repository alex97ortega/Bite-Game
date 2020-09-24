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

    public string infoAC, infoAD, infoAE;

    public GameObject panelHp;
    public GameObject barraVerdeHp;

    protected int initialHp;

    void Awake()
    {
        initialHp = hp;
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
}
