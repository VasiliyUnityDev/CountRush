using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject menu;
    public GameObject loseTable;
    public GameObject winTable;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void MenuClose()
    {
        menu.SetActive(false);
    }

    public void Lose()
    {
        loseTable.SetActive(true);
    }

    public void Win()
    {
        CharacterMovement.speedMove = 0f;

        winTable.SetActive(true);
    }
}
