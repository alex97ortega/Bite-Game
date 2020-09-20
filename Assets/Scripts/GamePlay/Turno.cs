using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turno : MonoBehaviour
{
    Color azul, rojo;
    public Image colorFondo,foto;
    public Text text;
    int id;

    // Start is called before the first frame update
    void Awake()
    {
        azul = new Color(49, 183, 190, 146);
        rojo = new Color(135, 40, 40, 146);
    }
    
    public void SetProperties(int _id, Image imgFoto)
    {
        id = _id;

        if (id >= FindObjectOfType<GameManager>().GetNumJugadoresCombate())
            colorFondo.color = rojo;

        foto = imgFoto;
    }
    public int GetId() { return id; }
}
