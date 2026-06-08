using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Wajib untuk komponen Image

public class UIAvatarManager : MonoBehaviour
{
    public static UIAvatarManager Instance { get; private set; }

    [Header("UI References")]
    public Image avatarImage;

    [Header("Expression Sprites")]
    public Sprite smileSprite;
    public Sprite calmSprite;
    public Sprite sadnessSprite;
    public Sprite aggressionSprite;

    // Variabel internal untuk melacak state visual saat ini
    private Sprite currentBaseSprite;
    private bool isShowingReaction = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Dengarkan perubahan status agar ekspresi dasar selalu sinkron dengan Sanity
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged += UpdateBaseExpression;
            UpdateBaseExpression(); // Set ekspresi pertama kali saat mulai
        }
    }

    private void OnDestroy()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged -= UpdateBaseExpression;
        }
    }

    // Fungsi ini dipanggil otomatis setiap kali ada status (termasuk Sanity) yang berubah
    private void UpdateBaseExpression()
    {
        int currentSanity = PlayerStats.Instance.currentSanity;

        // Tentukan ekspresi permanen berdasarkan kondisi Sanity
        if (currentSanity >= 70)
        {
            currentBaseSprite = smileSprite;
        }
        else if (currentSanity >= 30)
        {
            currentBaseSprite = calmSprite;
        }
        else
        {
            currentBaseSprite = aggressionSprite;
        }

        // Jika tidak sedang menunjukkan reaksi sementara, terapkan ekspresi dasar ini ke layar
        if (!isShowingReaction)
        {
            avatarImage.sprite = currentBaseSprite;
        }
    }

    // Fungsi untuk dipanggil oleh Kartu saat di-klik
    public void ShowCardReaction(bool isPositiveCard)
    {
        // Hentikan coroutine sebelumnya jika pemain nge-spam klik kartu dengan cepat
        StopAllCoroutines();
        StartCoroutine(ReactionRoutine(isPositiveCard));
    }

    // Coroutine untuk menahan ekspresi selama 1 detik tanpa membekukan game
    private IEnumerator ReactionRoutine(bool isPositiveCard)
    {
        isShowingReaction = true;

        // Tentukan sprite sementara berdasarkan sifat kartu
        if (isPositiveCard)
        {
            avatarImage.sprite = smileSprite;
        }
        else
        {
            avatarImage.sprite = sadnessSprite;
        }

        // Tunggu 1 detik
        yield return new WaitForSeconds(1f);

        // Kembalikan ke state permanen (berdasarkan sanity terkini)
        isShowingReaction = false;
        avatarImage.sprite = currentBaseSprite;
    }
}