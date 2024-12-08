using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class Aleatorias : MonoBehaviour
{
    public string imagesFolderPath = "Assets/Personajes";  // Cambié a la carpeta de personajes
    public string pistasFilePath = "Assets/Pistas/Pistas.csv";  // El archivo CSV con pistas de los personajes
    public List<Image> buttonImages;
    public List<TextMeshProUGUI> personajeTexts;  // Lista para los textos de los personajes
    public TextMeshProUGUI pistaText;
    public TextMeshProUGUI contadorText;
    public GameObject ganastePanel;
    public GameObject panelAjustes;
    public TextMeshProUGUI ganastepuntos;
    public Button descartarButton;

    private Dictionary<int, List<string>> pistasPorPersonaje = new Dictionary<int, List<string>>();  // Diccionario de pistas por personaje
    private Dictionary<int, string> personajes = new Dictionary<int, string>();  // Diccionario de personajes
    private int contador = 0;
    private int puntos = 0;
    private int extra = 100;
    private int penalizacion = 0;
    private int currentPersonajeID;
    private int currentPistaIndex = 0;
    private int selectedPersonajeID;
    private Sprite xSprite;
    private Button botonSeleccionado;

    void Start()
    {
        CargarPistasDesdeCSV();
        CargarPersonajes();  // Cargar los personajes
        AssignRandomImages();
        MostrarPistaDeImagenAleatoria();
        ganastePanel.SetActive(false);
        panelAjustes.SetActive(false);
        xSprite = LoadXSprite();
        descartarButton.onClick.AddListener(Descartar);
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
        SceneManager.LoadScene("Hdif");
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
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] lineData = csvLines[i].Split(',');

                if (int.TryParse(lineData[0].Trim(), out int personajeID))
                {
                    List<string> pistas = new List<string>();
                    for (int j = 2; j < lineData.Length; j++)
                    {
                        pistas.Add(lineData[j].Trim());
                    }
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

    void CargarPersonajes()
    {
        personajes = new Dictionary<int, string>()
        {
            { 1, "Abraham Lincoln" },
        { 2, "Alejandro Magno" },
        { 3, "Camarena" },
        { 4, "Cristobal Colon" },
        { 5, "Davinci" },
        { 6, "Einstein" },
        { 7, "Frida Kahlo" },
        { 8, "Galileo Galilei" },
        { 9, "Isaac Newton" },
        { 10, "Julio Cesar" },
        { 11, "Karl Marx" },
        { 12, "Napoleon Bonaparte" },
        { 13, "Nelson Mandela" },
        { 14, "Nikola Tesla" },
        { 15, "Pablo Picasso" },
        { 16, "Simon Bolivar" },
        { 17, "Sor Juana" },
        { 18, "William Shakespear" }
        };
    }

    void AssignRandomImages()
{
    List<Sprite> personajeImages = LoadSpritesFromFolder(imagesFolderPath);
    List<Sprite> shuffledImages = new List<Sprite>(personajeImages);
    ShuffleList(shuffledImages);

    // Asegúrate de que las listas tengan el mismo tamaño antes de acceder a ellas
    int buttonCount = buttonImages.Count;
    int imageCount = shuffledImages.Count;

    for (int i = 0; i < buttonCount; i++)
    {
        if (i < imageCount)  // Solo asignar si hay suficientes imágenes
        {
            buttonImages[i].sprite = shuffledImages[i];
            int personajeID = int.Parse(shuffledImages[i].name); // Usamos el nombre del archivo como ID del personaje
            personajeTexts[i].text = personajes[personajeID];  // Asignamos el nombre del personaje al texto
            Button button = buttonImages[i].GetComponent<Button>();
            button.onClick.AddListener(() => SeleccionarPersonaje(personajeID, button));
        }
        else
        {
            // Si no hay más imágenes, puedes desactivar los botones restantes o manejarlo de otra manera
            buttonImages[i].gameObject.SetActive(false);
            personajeTexts[i].text = "";  // Limpiar el texto si no hay suficiente imagen
        }
    }
}


    void SeleccionarPersonaje(int personajeID, Button boton)
    {
        selectedPersonajeID = personajeID;
        botonSeleccionado = boton;
    }

    public void Descartar()
    {
        if (botonSeleccionado != null)
        {
            if (xSprite != null)
            {
                botonSeleccionado.image.sprite = xSprite;
            }
            else
            {
                Debug.LogError("xSprite es nulo. Asegúrate de que la imagen X se ha cargado correctamente.");
            }
        }
        else
        {
            Debug.LogError("No se ha seleccionado ningún botón.");
        }
    }

    void MostrarPista(int personajeID)
    {
        if (pistasPorPersonaje.ContainsKey(personajeID))
        {
            List<string> pistas = pistasPorPersonaje[personajeID];
            if (currentPistaIndex < pistas.Count)
            {
                string pista = pistas[currentPistaIndex];
                pistaText.text = pista;
                contador++;
                UpdateContadorText();
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

    public void SiguientePista()
    {
        if (contador > 4) 
        {
            penalizacion = 100;  
            extra = 0; 
        } 
        else if (contador == 4)
        {
            extra = 0; 
        }

        currentPistaIndex++;
        MostrarPista(currentPersonajeID);
    }

    public void VerificarSeleccion()
    {
        if (contador > 4) 
        {
            penalizacion = 100;  
            extra = 0; 
        } 
        else if (contador == 4)
        {
            extra = 0; 
        }
        if (selectedPersonajeID == currentPersonajeID)
        {
            puntos = 500 + (extra / (int)Mathf.Pow(2, contador - 1)) - penalizacion;
            ganastePanel.SetActive(true);
            ganastepuntos.text = "Puntos: " + puntos;
            pistaText.text = "¡Has ganado!";
        } 
        else 
        {
            puntos = contador * 500 / 4 - penalizacion;
            if (puntos < 0) puntos = 0;
            pistaText.text = "Has perdido. Puntos: " + puntos;
        }
    }

    void UpdateContadorText()
    {
        contadorText.text = "Pistas: " + contador;
    }

    public void volverAjugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MostrarPistaDeImagenAleatoria()
    {
        int randomButtonIndex = Random.Range(0, buttonImages.Count);
        Image randomButtonImage = buttonImages[randomButtonIndex];

        if (int.TryParse(randomButtonImage.sprite.name, out int personajeID))
        {
            currentPersonajeID = personajeID;
            currentPistaIndex = 0;
            MostrarPista(currentPersonajeID);
        }
    }

    Sprite LoadXSprite()
    {
        string xImagePath = Path.Combine("Assets/Imagenes", "X.png");
        if (File.Exists(xImagePath))
        {
            byte[] fileData = File.ReadAllBytes(xImagePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("La imagen de la X no se encontró en: " + xImagePath);
            return null;
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
            sprite.name = Path.GetFileNameWithoutExtension(file);
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