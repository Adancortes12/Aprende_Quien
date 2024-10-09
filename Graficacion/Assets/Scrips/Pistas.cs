using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  // Para leer archivos
using TMPro;  // Para TextMeshPro

public class QuestionManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;  // Cambiar a TextMeshProUGUI
    private List<string> questions = new List<string>();
    private List<string> hints = new List<string>();

    void Start()
    {
        LoadQuestionsFromCSV("Assets/Pistas/Pistas.csv");
        ShowRandomQuestion();
    }

    void LoadQuestionsFromCSV(string path)
    {
        if (File.Exists(path))
        {
            string[] csvLines = File.ReadAllLines(path);
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] lineData = csvLines[i].Split(',');
                questions.Add(lineData[0]);
                hints.Add(lineData[1]);
            }
        }
        else
        {
            Debug.LogError("El archivo CSV no se encontró en: " + path);
        }
    }

    void ShowRandomQuestion()
    {
        int randomIndex = Random.Range(0, questions.Count);
        questionText.text = questions[randomIndex] + "\nPista: " + hints[randomIndex];
    }

    public void ShowNewRandomQuestion()
    {
        ShowRandomQuestion();
    }
}
