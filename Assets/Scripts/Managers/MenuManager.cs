using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject firstMenu, secondMenu;

    public void ChangeMenus()
    {
        firstMenu.SetActive(!firstMenu.activeSelf);
        secondMenu.SetActive(!secondMenu.activeSelf);
    }
}
