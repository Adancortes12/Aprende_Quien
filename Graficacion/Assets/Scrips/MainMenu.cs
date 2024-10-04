using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
 public void Iniciar()
    {
        SceneManager.LoadScene("Categorias");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }
}
