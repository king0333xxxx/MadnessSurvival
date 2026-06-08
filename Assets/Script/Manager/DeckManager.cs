using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("Deck Configuration")]
    [SerializeField] private List<CardData> startingDeck; // Kartu awal yang diatur dari Inspector

    // Piles (Runtime Data)
    private List<CardData> drawPile = new List<CardData>();
    private List<CardData> handPile = new List<CardData>();
    private List<CardData> discardPile = new List<CardData>();

    // Getters agar script lain (seperti UI) bisa membaca jumlah kartu tanpa bisa mengubahnya
    public List<CardData> HandPile => handPile;
    public int DrawPileCount => drawPile.Count;
    public int DiscardPileCount => discardPile.Count;

    // Events untuk memberi tahu UI jika ada perubahan tumpukan
    public event Action OnHandUpdated;
    public event Action OnPileSizesChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Dipanggil di awal permainan (misal saat GameManager masuk ke StartRun)
    public void InitializeDeck()
    {
        drawPile.Clear();
        handPile.Clear();
        discardPile.Clear();

        // WAJIB: Copy referensi kartu dari startingDeck ke drawPile, jangan langsung menyamakan list-nya
        drawPile.AddRange(startingDeck);

        ShuffleDrawPile();
        OnPileSizesChanged?.Invoke();
    }

    // Algoritma Fisher-Yates Shuffle (Nilai tambah besar di mata Senior Programmer karena optimal O(n))
    private void ShuffleDrawPile()
    {
        System.Random rng = new System.Random();
        int n = drawPile.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardData value = drawPile[k];
            drawPile[k] = drawPile[n];
            drawPile[n] = value;
        }
    }

    // Fungsi untuk menarik sejumlah kartu
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Jika Draw Pile habis, kocok kembali Discard Pile ke dalam Draw Pile
            if (drawPile.Count == 0)
            {
                if (discardPile.Count == 0)
                {
                    Debug.Log("Semua tumpukan kartu habis!");
                    break; // Tidak ada kartu tersisa sama sekali
                }
                ResetDrawPileFromDiscard();
            }

            // Ambil kartu paling atas (indeks 0)
            CardData cardToDraw = drawPile[0];
            drawPile.RemoveAt(0);
            handPile.Add(cardToDraw);
        }

        OnHandUpdated?.Invoke();
        OnPileSizesChanged?.Invoke();
    }

    private void ResetDrawPileFromDiscard()
    {
        Debug.Log("Draw pile habis. Mengocok kembali discard pile...");
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        ShuffleDrawPile();
    }

    // Dipanggil saat kartu sukses dimainkan oleh Player
    public void PlayCard(CardData card)
    {
        if (handPile.Contains(card))
        {
            handPile.Remove(card);

            // Masukkan ke discard pile (Kecuali jika ada logika khusus kartu tertentu hancur permanen)
            discardPile.Add(card);

            OnHandUpdated?.Invoke();
            OnPileSizesChanged?.Invoke();
        }
    }

    // Dipanggil saat End Turn (Semua kartu di tangan dibuang ke discard pile)
    public void DiscardHand()
    {
        // Gunakan loop terbalik atau list sementara karena kita akan memodifikasi list saat looping
        List<CardData> cardsToDiscard = new List<CardData>(handPile);

        foreach (CardData card in cardsToDiscard)
        {
            // LOGIKA KHUSUS HORROR: Kartu jenis "Curse" tidak bisa dibuang otomatis saat End Turn!
            if (card.cardType == CardType.Curse)
            {
                continue; // Lewati kartu curse, biarkan tetap bertahan di tangan player
            }

            handPile.Remove(card);
            discardPile.Add(card);
        }

        OnHandUpdated?.Invoke();
        OnPileSizesChanged?.Invoke();
    }

    // Fungsi untuk memasukkan kartu baru ke dalam dek
    public void AddCardToDeck(CardData newCard)
    {
        // Masukkan ke discard pile agar akan ter-shuffle masuk ke draw pile nanti
        discardPile.Add(newCard);

        // Opsional: Masukkan ke startingDeck jika kamu ingin kartu ini permanen terbawa kalau player "Restart Game"
        startingDeck.Add(newCard);

        OnPileSizesChanged?.Invoke();
        Debug.Log($"Kartu {newCard.cardName} berhasil ditambahkan ke dek!");
    }

    public void TriggerHandUpdate()
    {
        OnHandUpdated?.Invoke();
    }
}