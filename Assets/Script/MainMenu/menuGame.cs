using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuGame : MonoBehaviour
{
    [Header("UI Loading Screen")]
    [Tooltip("Masukkan Panel Loading Screen di sini")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;

    [Header("UI Yang Disembunyikan Saat Loading")]
    [Tooltip("Masukkan panel apa saja yang ingin dihilangkan saat loading (Misal: Main Menu, Pause Menu, HUD Gameplay, dll)")]
    [SerializeField] private GameObject[] panelsToHide;

    // --- FUNGSI UNIVERSAL UNTUK PINDAH SCENE ---

    // Fungsi utama untuk pindah scene menggunakan Loading Screen
    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        // Kembalikan waktu ke normal (Penting jika pindah scene dari posisi Pause)
        Time.timeScale = 1;

        // Sembunyikan semua panel yang ada di array
        foreach (GameObject panel in panelsToHide)
        {
            if (panel != null) panel.SetActive(false);
        }

        // Tampilkan loading screen
        if (loadingScreen != null) loadingScreen.SetActive(true);

        StartCoroutine(LoadAsynchronously(sceneName));
    }

    // Fungsi utama untuk pindah scene langsung (Tanpa Loading Screen)
    public void LoadSceneNormal(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    // --- MEMPERTAHANKAN NAMA FUNGSI LAMA AGAR TOMBOL DI UNITY TIDAK ERROR ---
    // Fungsi-fungsi di bawah ini memanggil fungsi universal di atas
    
    public void StartGame(string sceneName) => LoadSceneWithLoadingScreen(sceneName);
    
    public void BackMenu(string sceneName) => LoadSceneWithLoadingScreen(sceneName);
    
    public void KembaliMenu(string sceneName) => LoadSceneNormal(sceneName);

    // ------------------------------------------------------------------------

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Keluar"); // Menandakan di console bahwa tombol exit berfungsi
    }

    // Proses loading secara Asynchronous
    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Menahan scene agar tidak langsung aktif otomatis saat loading mencapai 90%
        operation.allowSceneActivation = false;

        float targetProgress = 0f;

        while (!operation.isDone)
        {
            // Ambil progress asli dari Unity (0 sampai 0.9)
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Buat pergerakan nilai slider berjalan halus, tidak melompat kaku
            targetProgress = Mathf.MoveTowards(targetProgress, realProgress, Time.deltaTime * 2f);

            if (loadingSlider != null)
            {
                loadingSlider.value = targetProgress;
            }

            // Jika slider visual sudah penuh mencapai 100% dan Unity selesai memuat data di background
            if (targetProgress >= 1f && operation.progress >= 0.9f)
            {
                // Opsi A: Langsung pindah otomatis dengan jeda halus
                operation.allowSceneActivation = true;

                // Opsi B: Jika ingin ada mekanik "Press Any Key to Continue" tinggal disisipkan di sini:
                // if (Input.anyKey) operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}