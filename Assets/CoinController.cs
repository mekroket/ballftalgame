using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float rotationSpeed = 100f; // Dönme hızı
    
    void Update()
    {
        // Altını sürekli döndür
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Eğer top altına değerse
        if (other.CompareTag("Player"))
        {
            // Ses efektini çal
            BallController ballController = other.GetComponent<BallController>();
            if (ballController != null)
            {
                ballController.PlayCoinSound();
            }

            // Skoru artır
            GameManager.Instance.CollectCoin();
            
            // Altını yok et
            Destroy(gameObject);
        }
    }
} 