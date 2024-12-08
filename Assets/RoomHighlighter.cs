using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RoomHighlighter : MonoBehaviour
{
    public GameObject plantaUI; // Referência para a planta no Canvas
    public GameObject highlight; // Quadrado vermelho
    private ARTrackedImageManager trackedImageManager;

    private string currentRoomId = ""; // Para evitar redundância de destaque

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Quando uma nova imagem for detectada ou atualizada
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateRoomHighlight(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateRoomHighlight(trackedImage);
        }
    }

    void UpdateRoomHighlight(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            string roomId = trackedImage.referenceImage.name;
            
            Debug.Log("Cômodo detectado: " + roomId);

            // Evita reprocessar o mesmo cômodo
            if (roomId == currentRoomId) return;

            currentRoomId = roomId;

            // Atualiza o quadrado de destaque baseado no cômodo
            switch (roomId)
            {
                case "cozinha":
                    highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -100); // Ajuste conforme o cômodo
                    highlight.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200); // Tamanho do quadrado
                    break;

                case "sala":
                    highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -50);
                    highlight.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
                    break;

                // Adicione mais casos para outros cômodos
            }

            // Certifique-se de que o quadrado e a planta estejam visíveis
            if (!plantaUI.activeSelf) plantaUI.SetActive(true);
            if (!highlight.activeSelf) highlight.SetActive(true);
        }
        else
        {
            // Se o QR code não estiver sendo rastreado, esconda o quadrado
            highlight.SetActive(false);
        }
    }
}