using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Galagedon.Estructuras;
using Galagedon.DataStructures.CoreStructures;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager Instance { get; private set; }
    private AVL<ScoreEntry> scoreboardTree;

    [Header("Configuración de UI")]
    [SerializeField] private GameObject panelScoreboard;
    [SerializeField] private TextMeshProUGUI textoListaPuntajes;

private void Start()
    {
        scoreboardTree = new AVL<ScoreEntry>();

        // ejemplop :v
        AddScoreToBoard("PRO", 2500);
        AddScoreToBoard("HACKER xD ", 3200);
        AddScoreToBoard("NOOB", 800);
        int puntajeObtenido = PlayerPrefs.GetInt("PuntajeFinal", 0);
        string nombrePiloto = "Yop";
        AddScoreToBoard(nombrePiloto, puntajeObtenido);

        MostrarScoreboard();
    }

    public void AddScoreToBoard(string name, int score)
    {
        scoreboardTree.Insert(new ScoreEntry(name, score));
    }
    //esto s llama cuando termina el juego nomais, hay que aplicarlo a la victoria tmbn
    public void MostrarScoreboard()
    {
        List<ScoreEntry> topScores = scoreboardTree.GetDescendingElements();

        textoListaPuntajes.text = "=== TABLA DE POSICIONES ===\n\n";
        int rank = 1;

        foreach (ScoreEntry entry in topScores)
        {
            textoListaPuntajes.text += $"#{rank} - {entry.PlayerName}: {entry.Points} pts\n";
            rank++;
            if (rank > 7) break;
        }
    }
}