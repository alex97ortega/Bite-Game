using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayers : MonoBehaviour
{
    int numJugadores;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        numJugadores = gm.GetNumJugadoresCombate() - gm.GetNumJugadoresSeleccionados();
        GetComponent<Text>().text = "Selecciona " + numJugadores + " jugadores";
    }

    // Update is called once per frame
    void Update()
    {
        if(numJugadores != gm.GetNumJugadoresCombate() - gm.GetNumJugadoresSeleccionados())
        {
            numJugadores = gm.GetNumJugadoresCombate() - gm.GetNumJugadoresSeleccionados();
            if(numJugadores == 1)
                GetComponent<Text>().text = "Selecciona 1 jugador";
            else
                GetComponent<Text>().text = "Selecciona " + numJugadores + " jugadores";
        }
    }
}
