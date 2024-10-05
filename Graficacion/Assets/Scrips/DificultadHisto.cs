using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DificultadHisto : MonoBehaviour
{
    public void Back(){
    SceneManager.LoadScene("Categorias");
   }
   public void facil(){
    SceneManager.LoadScene("facil");
   }
}
