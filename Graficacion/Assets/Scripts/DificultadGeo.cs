using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DificultadGeo : MonoBehaviour
{
     public GameObject panelAjustes;
    public void Back(){
    SceneManager.LoadScene("Categorias");
   }
   public void facill(){
    SceneManager.LoadScene("facil");
   }
   public void Dificil(){
    SceneManager.LoadScene("Dificil");
   }
   public void Medio(){
    SceneManager.LoadScene("Medio");
   }
   public void Ajustes()
    {
        panelAjustes.SetActive(true); // Activa el panel 
    }

    public void cerrar()
    {
        panelAjustes.SetActive(false); // Desactiva el panel 
    }

    public void salir()
    {
        SceneManager.LoadScene("Gdif");
    }
}
