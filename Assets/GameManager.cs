using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI scoreText; // Skor metni için UI elemanı
    private int coinCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("GameManager başlatıldı");
        }
        else
        {
            Destroy(gameObject);
        }

        // UI kurulumu
        SetupUI();
    }

    void Start()
    {
        UpdateScoreText();
        Debug.Log("Başlangıç skoru: " + coinCount);
    }

    void SetupUI()
    {
        if (scoreText != null)
        {
            Debug.Log("Score Text bulundu");
            // Text ayarları
            scoreText.fontSize = 36;
            scoreText.fontStyle = FontStyles.Bold;
            scoreText.color = Color.white;
            scoreText.alignment = TextAlignmentOptions.Right;

            // RectTransform ayarları
            RectTransform rectTransform = scoreText.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1); // Sağ üst köşe
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(-20, -20);

            // Arka plan oluştur
            CreateBackgroundPanel();
        }
        else
        {
            Debug.LogError("Score Text atanmamış!");
        }
    }

    void CreateBackgroundPanel()
    {
        // Arka plan paneli oluştur
        GameObject panel = new GameObject("ScoreBackground");
        panel.transform.SetParent(scoreText.transform.parent);
        panel.transform.SetAsFirstSibling(); // Metnin arkasına al

        // Image component'i ekle
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0); // Şeffaf arka plan

        // RectTransform ayarları
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = scoreText.rectTransform.anchorMin;
        panelRect.anchorMax = scoreText.rectTransform.anchorMax;
        panelRect.pivot = scoreText.rectTransform.pivot;
        panelRect.anchoredPosition = scoreText.rectTransform.anchoredPosition;
        panelRect.sizeDelta = new Vector2(200, 50); // Panel boyutu
    }

    public void CollectCoin()
    {
        coinCount++;
        Debug.Log("Altın toplandı! Yeni skor: " + coinCount);
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = coinCount.ToString();
            Debug.Log("Skor metni güncellendi: " + scoreText.text);
        }
        else
        {
            Debug.LogError("Score Text null! Skor güncellenemedi.");
        }
    }
} 