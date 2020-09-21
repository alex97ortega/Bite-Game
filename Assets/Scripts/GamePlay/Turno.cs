﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turno : MonoBehaviour
{
    public Image colorAzul, colorRojo,foto;
    public Text text;
    int id;
    
    
    public void SetProperties(int _id, Sprite imgFoto)
    {
        id = _id;

        if (id < FindObjectOfType<GameManager>().GetNumJugadoresCombate())
            colorAzul.gameObject.SetActive(true);
        else
            colorRojo.gameObject.SetActive(true);

        foto.sprite = imgFoto;
    }
    public int GetId() { return id; }
    public void ActivarTexto()
    {
        text.gameObject.SetActive(true);
    }
    public void DesctivarTexto()
    {
        text.gameObject.SetActive(false);
    }
    public void Deslizar() { transform.position -= new Vector3(120, 0, 0); }
}
