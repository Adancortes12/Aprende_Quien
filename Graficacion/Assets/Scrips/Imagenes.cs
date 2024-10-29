using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class Aleatorias : MonoBehaviour
{
    public string imagesFolderPath = "Assets/Personajes";  // Ruta de la carpeta de imágenes
    public string pistasFilePath = "Assets/Pistas/Pistas.csv";  // Ruta del archivo CSV de pistas
    public List<Image> buttonImages;  // Lista de componentes Image de los botones
    public TextMeshProUGUI pistaText;  // Texto para mostrar la pista
    public Button siguientePistaButton;  // Botón para mostrar la siguiente pista

    private Dictionary<int, List<string>> pistasPorPersonaje = new Dictionary<int, List<string>>();  // Diccionario de pistas por ID
    private int personajeIDActual;  // ID del personaje actual
    private int pistaIndexActual;  // Índice de la pista actual

    void Start()
    {
        CargarPistasDesdeCSV();  // Cargar las pistas desde el archivo CSV
        AssignRandomImages();  // Asignar imágenes aleatorias a los botones

        // Mostrar automáticamente una pista para una de las imágenes al iniciar
        MostrarPistaDeImagenAleatoria();

        // Asignar el método siguientePista al botón
        siguientePistaButton.onClick.AddListener(SiguientePista);
    }

    public void Back()
    {
        SceneManager.LoadScene("Hdif");
    }

    void CargarPistasDesdeCSV()
    {
        if (File.Exists(pistasFilePath))
        {
            string[] csvLines = File.ReadAllLines(pistasFilePath);
            for (int i = 1; i < csvLines.Length; i++)  // Ignora la primera línea si es un encabezado
            {
                string[] lineData = csvLines[i].Split(',');

                if (int.TryParse(lineData[0].Trim(), out int personajeID))
                {
                    // Lista de pistas para el personaje
                    List<string> pistas = new List<string>();
                    for (int j = 2; j < lineData.Length; j++)  // Empieza desde el índice 2
                    {
                        pistas.Add(lineData[j].Trim());
                    }

                    // Agrega el ID del personaje y sus pistas al diccionario
                    pistasPorPersonaje[personajeID] = pistas;
                }
                else
                {
                    Debug.LogError("ID de personaje inválido en la línea: " + (i + 1));
                }
            }
        }
        else
        {
            Debug.LogError("El archivo de pistas no se encontró en: " + pistasFilePath);
        }
    }

    void AssignRandomImages()
    {
        List<Sprite> characterImages = LoadSpritesFromFolder(imagesFolderPath);
        List<Sprite> shuffledImages = new List<Sprite>(characterImages);
        ShuffleList(shuffledImages);

        // Asignar las imágenes a los botones y configurar eventos para mostrar pistas
        for (int i = 0; i < buttonImages.Count; i++)
        {
            if (i < shuffledImages.Count)
            {
                buttonImages[i].sprite = shuffledImages[i];
                
                // Convertir el nombre de la imagen (número) a un entero para buscar en el diccionario
                if (int.TryParse(shuffledImages[i].name, out int personajeID))
                {
                    buttonImages[i].GetComponent<Button>().onClick.AddListener(() => MostrarPista(personajeID));
                }
            }
        }
    }

    void MostrarPista(int personajeID)
    {
        if (pistasPorPersonaje.ContainsKey(personajeID))
        {
            personajeIDActual = personajeID;  // Guardar el ID del personaje actual
            pistaIndexActual = 0;  // Reiniciar el índice de la pista

            List<string> pistas = pistasPorPersonaje[personajeID];
            string pista = pistas[pistaIndexActual];  // Selecciona la primera pista
            pistaText.text = pista;
        }
        else
        {
            pistaText.text = "Pista no disponible para este personaje.";
        }
    }

    void MostrarPistaDeImagenAleatoria()
    {
        // Selecciona una imagen aleatoria de los botones y muestra la pista correspondiente
        int randomButtonIndex = Random.Range(0, buttonImages.Count);
        Image randomButtonImage = buttonImages[randomButtonIndex];

        if (int.TryParse(randomButtonImage.sprite.name, out int personajeID))
        {
            MostrarPista(personajeID);  // Usa el ID numérico de la imagen
        }
    }

    void SiguientePista()
    {
        if (pistasPorPersonaje.ContainsKey(personajeIDActual))
        {
            List<string> pistas = pistasPorPersonaje[personajeIDActual];
            pistaIndexActual = (pistaIndexActual + 1) % pistas.Count;  // Avanza al siguiente índice y reinicia si llega al final
            string pista = pistas[pistaIndexActual];
            pistaText.text = pista;
        }
        else
        {
            pistaText.text = "Pista no disponible para este personaje.";
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
            sprite.name = Path.GetFileNameWithoutExtension(file);  // Usar el nombre del archivo como nombre del sprite
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
