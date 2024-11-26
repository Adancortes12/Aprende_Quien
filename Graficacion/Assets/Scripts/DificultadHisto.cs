using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DificultadHisto : MonoBehaviour
{
    public void Back(){
    SceneManager.LoadScene("Categorias");
   }
   public void facill(){
    SceneManager.LoadScene("facil");
   }
    // Referencia al panel de dificultad "medio"
    public GameObject panelMedio;
    public GameObject panelFacil;
    public GameObject panelDificil;
     public GameObject panelAjustes;

    // Método para mostrar el panel de dificultad media
    public void Medio()
    {
        panelMedio.SetActive(true); // Activa el panel 
    }
    public void facil()
    {
        panelFacil.SetActive(true); // Activa el panel 
    }
    public void Dificil()
    {
        panelDificil.SetActive(true); // Activa el panel 
    }
    public void Ajustes()
    {
        panelAjustes.SetActive(true); // Activa el panel 
    }
    public void salir()
    {
        panelAjustes.SetActive(false); // Desactiva el panel 
    }
}
