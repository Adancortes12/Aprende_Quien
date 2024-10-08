using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DificultadHisto : MonoBehaviour
{
    public void Back(){
    SceneManager.LoadScene("Categorias");
   }
    // Referencia al panel de dificultad "medio"
    public GameObject panelMedio;
    public GameObject panelFacil;
    public GameObject panelDificil;


    // Método para mostrar el panel de dificultad media
    public void Medio()
    {
        panelMedio.SetActive(true); // Activa el panel medio
    }
    public void facil()
    {
        panelFacil.SetActive(true); // Activa el panel medio
    }
    public void Dificil()
    {
        panelDificil.SetActive(true); // Activa el panel medio
    }
}
