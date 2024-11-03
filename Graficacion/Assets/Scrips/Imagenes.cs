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
    public TextMeshProUGUI contadorText; // Texto para mostrar el contador de pistas
    public GameObject ganastePanel; // Panel "Ganaste"

    private Dictionary<int, List<string>> pistasPorPersonaje = new Dictionary<int, List<string>>();  // Diccionario de pistas por ID
    private int contadorPistas = 0;  // Contador de pistas vistas
    private int currentPersonajeID;  // ID del personaje actual
    private int currentPistaIndex = 0;  // Índice de la pista actual
    private int selectedPersonajeID; // ID del personaje seleccionado por el jugador

    void Start()
    {
        CargarPistasDesdeCSV();  // Cargar las pistas desde el archivo CSV
        AssignRandomImages();  // Asignar imágenes aleatorias a los botones

        // Mostrar automáticamente una pista para una de las imágenes al iniciar
        MostrarPistaDeImagenAleatoria();
        ganastePanel.SetActive(false); // Asegurarse de que el panel "Ganaste" esté oculto al iniciar
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
                    int id = personajeID; // Captura la variable para usar en el lambda
                    buttonImages[i].GetComponent<Button>().onClick.AddListener(() => SeleccionarPersonaje(id));
                }
            }
        }
    }

    void SeleccionarPersonaje(int personajeID)
    {
        selectedPersonajeID = personajeID;
        // No hacer nada más cuando se selecciona un personaje, solo almacenar el ID
    }

    void MostrarPista(int personajeID)
    {
        if (pistasPorPersonaje.ContainsKey(personajeID))
        {
            List<string> pistas = pistasPorPersonaje[personajeID];
            if (currentPistaIndex < pistas.Count)
            {
                string pista = pistas[currentPistaIndex];  // Obtiene la pista actual
                pistaText.text = pista;
                contadorPistas++;
                UpdateContadorText();  // Actualiza el texto del contador
            }
            else
            {
                pistaText.text = "No hay más pistas disponibles.";
            }
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
            currentPersonajeID = personajeID;  // Almacena el ID del personaje actual
            currentPistaIndex = 0;  // Reinicia el índice de la pista
            MostrarPista(currentPersonajeID);  // Usa el ID numérico de la imagen
        }
    }

    public void SiguientePista()
    {
        if (pistasPorPersonaje.ContainsKey(currentPersonajeID))
        {
            List<string> pistas = pistasPorPersonaje[currentPersonajeID];
            currentPistaIndex++;  // Incrementa el índice de la pista

            if (currentPistaIndex < pistas.Count)
            {
                string pista = pistas[currentPistaIndex];  // Obtiene la siguiente pista
                pistaText.text = pista;
                contadorPistas++;
                UpdateContadorText();  // Actualiza el texto del contador
            }
            else
            {
                currentPistaIndex--;  // Mantén el índice en el límite si ya no hay más pistas
                pistaText.text = "No hay más pistas disponibles.";
            }
        }
    }

    void UpdateContadorText()
    {
        int puntos = 500 - (contadorPistas * 10);
        if (puntos < 0) puntos = 0; // Asegúrate de que los puntos no sean negativos
        contadorText.text = "Puntos: " + puntos;
    }

    public void VerificarSeleccion()
    {
        if (selectedPersonajeID == currentPersonajeID)
        {
            ganastePanel.SetActive(true); // Muestra el panel "Ganaste"
        }
        else
        {
            // Aquí podrías agregar lógica para indicar que la selección fue incorrecta
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

