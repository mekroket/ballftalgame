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
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // UI kurulumu
        SetupUI();
    }

    void Start()
    {
        UpdateScoreText();
    }

    void SetupUI()
    {
        if (scoreText != null)
        {
            // Text ayarları
            scoreText.fontSize = 36;
            scoreText.fontStyle = FontStyles.Bold;
            scoreText.color = Color.white;
            scoreText.alignment = TextAlignmentOptions.Right;

            // RectTransform ayarları
            RectTransform rectTransform = scoreText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(1, 1); // Sağ üst köşe
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(1, 1);
                rectTransform.anchoredPosition = new Vector2(-20, -20);
            }

            // Arka plan oluştur
            CreateBackgroundPanel();
        }
    }

    void CreateBackgroundPanel()
    {
        if (scoreText == null || scoreText.transform.parent == null)
        {
            return;
        }

        // Arka plan paneli oluştur
        GameObject panel = new GameObject("ScoreBackground");
        panel.transform.SetParent(scoreText.transform.parent);
        panel.transform.SetAsFirstSibling(); // Metnin arkasına al

        // Image component'i ekle
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0); // Şeffaf arka plan

        // RectTransform ayarları
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        if (panelRect != null && scoreText.rectTransform != null)
        {
            panelRect.anchorMin = scoreText.rectTransform.anchorMin;
            panelRect.anchorMax = scoreText.rectTransform.anchorMax;
            panelRect.pivot = scoreText.rectTransform.pivot;
            panelRect.anchoredPosition = scoreText.rectTransform.anchoredPosition;
            panelRect.sizeDelta = new Vector2(200, 50); // Panel boyutu
        }
    }

    public void CollectCoin()
    {
        coinCount++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = coinCount.ToString();
        }
    }
} 