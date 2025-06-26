using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Top objesi
    public float distance = 5.0f; // Kamera mesafesi (daha yakın)
    public float height = 2.0f; // Kamera yüksekliği (daha alçak)
    public float rotationSpeed = 0.5f; // Daha yavaş dönüş
    public float smoothSpeed = 5.0f; // Kamera takip yumuşaklığı
    public float minVerticalAngle = -30f; // En düşük dikey açı
    public float maxVerticalAngle = 60f; // En yüksek dikey açı
    
    private float currentRotation = 0f;
    private float currentVerticalAngle = 0f;
    private Vector3 smoothVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Fare ile yatay ve dikey dönüş
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Dikey açıyı sınırla
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle - mouseY, minVerticalAngle, maxVerticalAngle);
        currentRotation += mouseX;

        // Kamera pozisyonunu hesapla
        Vector3 targetPosition = target.position;
        
        // Kamera rotasyonunu hesapla
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentRotation, 0);
        Vector3 negDistance = new Vector3(0, height, -distance);
        Vector3 position = rotation * negDistance + targetPosition;

        // Yumuşak geçişle kamera pozisyonunu güncelle
        transform.position = Vector3.SmoothDamp(transform.position, position, ref smoothVelocity, Time.deltaTime * smoothSpeed);
        
        // Kamerayı hedefe döndür
        transform.LookAt(targetPosition + Vector3.up * height * 0.5f);
    }
} 