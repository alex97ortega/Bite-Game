using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public Text textoApagado, textoEncendido;

    // Start is called before the first frame update
    void Start()
    {
        textoApagado.text = "";
        textoEncendido.text = "";
    }

    public void LanzaLog(string mensaje)
    {
        textoApagado.text = textoEncendido.text;
        textoEncendido.text = mensaje;
    }
}
