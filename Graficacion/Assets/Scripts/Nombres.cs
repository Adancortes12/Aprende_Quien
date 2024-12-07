using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.IO;

public class Nombrew: MonoBehaviour
{
    public string imagesFolderPath = "Assets/Banderas"; // Ruta de la carpeta de imágenes
    public List<Image> buttonImages; // Lista de imágenes de botones (P1, P2, P3, P4)
    public List<TextMeshProUGUI> paisTexts; // Lista de TextMeshPro para mostrar los nombres de los países (TP1, TP2, TP3, TP4)

    private Dictionary<int, string> paises = new Dictionary<int, string>
    {
        {1, "Alemania"},
        {2, "Arabia Saudita"},
        {3, "Argentina"},
        {4, "Australia"},
        {5, "Brasil"},
        {6, "Canadá"},
        {7, "China"},
        {8, "Colombia"},
        {9, "Egipto"},
        {10, "España"},
        {11, "Etiopía"},
        {12, "Fiyi"},
        {13, "Francia"},
        {14, "Italia"},
        {15, "Japón"},
        {16, "Nueva Zelanda"},
        {17, "Papúa Nueva Guinea"},
        {18, "Reino Unido"},
        {19, "Samoa"},
        {20, "Vietnam"}
    };

    void Start()
    {
        AsignarNombrePaises();
    }

    void AsignarNombrePaises()
    {
        List<Sprite> banderas = LoadSpritesFromFolder(imagesFolderPath);

        if (banderas.Count < buttonImages.Count)
        {
            Debug.LogError("No hay suficientes imágenes en la carpeta para asignar a todos los botones.");
            return;
        }

        for (int i = 0; i < buttonImages.Count; i++)
        {
            int randomIndex = Random.Range(0, banderas.Count);
            Sprite bandera = banderas[randomIndex];
            buttonImages[i].sprite = bandera;

            string nombreArchivo = Path.GetFileNameWithoutExtension(bandera.name); // Nombre del archivo sin extensión
            if (int.TryParse(nombreArchivo, out int numeroImagen) && paises.ContainsKey(numeroImagen))
            {
                paisTexts[i].text = paises[numeroImagen]; // Asignar el nombre del país a la etiqueta
            }
            else
            {
                paisTexts[i].text = "Desconocido"; // En caso de error
            }

            banderas.RemoveAt(randomIndex); // Remover la imagen ya asignada
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
            sprite.name = Path.GetFileNameWithoutExtension(file); // Asignar el nombre del archivo como nombre del sprite
            sprites.Add(sprite);
        }

        return sprites;
    }
}