using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Categorias : MonoBehaviour
{
   public void Back(){
    SceneManager.LoadScene("Menu");
   }
   public void Historia(){
    SceneManager.LoadScene("Hdif");
   }

   public void Geografia(){
      SceneManager.LoadScene("Gdif");
   }
   
}
