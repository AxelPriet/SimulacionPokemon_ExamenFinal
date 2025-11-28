using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Camera cam;

    [Header("Movimiento")]
    public float moveSpeed = 10f;
    private Vector2 cameraMoveInput;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    private float zoomInput;

    [Header("Recentrar")]
    public bool followPlayer = false;

    private float cameraHeight;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("CameraMovement: No se encontró la cámara principal.");
            enabled = false;
            return;
        }

        cameraHeight = cam.transform.position.z;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnCameraMove += HandleCameraMove;
            InputManager.Instance.OnCameraZoom += HandleZoom;
            InputManager.Instance.OnRecenterCamera += HandleRecenter;
        }
    }

    void HandleCameraMove(Vector2 input)
    {
        followPlayer = false;
        cameraMoveInput = input;
    }

    void HandleZoom(float input)
    {
        zoomInput = input;
    }

    void HandleRecenter()
    {
        followPlayer = !followPlayer;
        if (player != null && followPlayer) StartCoroutine(RecenterSmooth());
    }

    IEnumerator RecenterSmooth()
    {
        while (followPlayer)
        {
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, cameraHeight);
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, 5f * Time.deltaTime);

            followPlayer = !(Vector3.Distance(transform.position, targetPos) < 0.1f);

            yield return null;
        }
    }

    void Update()
    {
        if (cam == null) return;

        if (followPlayer && player != null)
        {
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, cameraHeight);
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, 5f * Time.deltaTime);
        }
        else
        {
            Vector3 movement = new Vector3(cameraMoveInput.x, cameraMoveInput.y, cameraHeight);
            cam.transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }

        cam.orthographicSize -= zoomInput * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}

