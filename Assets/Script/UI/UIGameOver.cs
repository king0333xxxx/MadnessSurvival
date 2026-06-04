using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    public static UIGameOver Instance { get; private set; }

    [Header("UI Text References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    [Header("Button References")]
    public GameObject endlessButton;   // Tombol Try Endless
    public GameObject restartButton;   // Tombol Restart biasa
    public GameObject mainMenuButton;  // Tombol Main Menu

    private void Awake()
    {
        Instance = this;
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(bool isWin)
    {
        gameOverPanel.SetActive(true);

        if (isWin)
        {
            titleText.text = "SURVIVED";
            titleText.color = Color.yellow;
            descriptionText.text = "Kamu berhasil selamat... tapi apakah kamu berani lanjut?";

            // Munculkan tombol Endless, sembunyikan Restart agar pemain memilih Endless atau Main Menu
            endlessButton.SetActive(true);
            restartButton.SetActive(false);
            mainMenuButton.SetActive(true);
        }
        else
        {
            titleText.text = "CONSUMED";
            titleText.color = Color.red;
            descriptionText.text = "Kewarasan dan fisikmu hancur ditelan kegelapan...";

            // Sembunyikan tombol Endless kalau kalah
            endlessButton.SetActive(false);
            restartButton.SetActive(true);
            mainMenuButton.SetActive(true);
        }
    }

    // Fungsi untuk tombol Endless
    public void SelectEndlessMode()
    {
        gameOverPanel.SetActive(false); // Sembunyikan layar Game Over
        GameManager.Instance.StartEndlessMode(); // Beritahu GameManager untuk lanjut
    }

    // Fungsi untuk tombol Restart
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Fungsi untuk tombol Main Menu
    public void GoToMainMenu()
    {
        // Pastikan kamu punya scene bernama "MainMenu" di Build Settings
        // Jika belum ada, sementara gunakan Reload Scene saja
        SceneManager.LoadScene("MainMenu");
    }
}