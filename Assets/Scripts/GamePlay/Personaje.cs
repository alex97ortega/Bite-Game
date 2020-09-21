using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    int casillaX, casillaZ;
    public int id;
    public Sprite foto;
    public bool chico;
    public bool deportista;
    public bool gaymer;
    public bool fumador;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
    public void SetColor(Material color)
    {
        foreach (var i in GetComponentsInChildren<MeshRenderer>())
        {
            if(i.gameObject.tag != "Cabeza" && i.gameObject.tag != "Unia")
                i.material = color;
        }
    }
    public Sprite GetFoto() { return foto; }
}
