using UnityEngine;
using UnityEngine.UI; // Agrega esta línea
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class Aleatorias : MonoBehaviour
{
     public string imagesFolderPath = "Assets/Personajes"; // Ruta de la carpeta de imágenes
    public List<Image> buttonImages; // Lista de componentes Image de los botones a los que se asignarán las imágenes
    
    public void Back()
    {
        SceneManager.LoadScene("Hdif");
    }

    void Start()
    {
        AssignRandomImages();
    }

    void AssignRandomImages()
    {
        // Cargar las imágenes desde la carpeta
        List<Sprite> characterImages = LoadSpritesFromFolder(imagesFolderPath);

        // Mezcla las imágenes de los personajes
        List<Sprite> shuffledImages = new List<Sprite>(characterImages);
        ShuffleList(shuffledImages);

        // Asigna las imágenes a los componentes Image de los botones
        for (int i = 0; i < buttonImages.Count; i++)
        {
            if (i < shuffledImages.Count)
            {
                buttonImages[i].sprite = shuffledImages[i];
            }
        }
    }

    List<Sprite> LoadSpritesFromFolder(string folderPath)
    {
        List<Sprite> sprites = new List<Sprite>();
        string[] files = Directory.GetFiles(folderPath, "*.png");

        foreach (string file in files)
        {
            byte[] fileData = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprites.Add(sprite);
        }

        return sprites;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
