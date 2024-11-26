using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DificultadGeo : MonoBehaviour
{
    public void Back(){
    SceneManager.LoadScene("Categorias");
   }
   public void facill(){
    SceneManager.LoadScene("facil");
   }
}
