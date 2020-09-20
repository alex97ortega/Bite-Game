using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void SelectNumPlayers(int players)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            gm.SetNumJugadoresCombate(players);
            ChangeScene("SelectPlayers");
        }
        else
            Debug.Log("No hay GM!!");
    }
}
