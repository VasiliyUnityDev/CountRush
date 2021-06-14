using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int whatScene;

    public void OnClick()
    {
        SceneManager.LoadScene(whatScene);
    }
}
