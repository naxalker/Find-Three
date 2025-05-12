using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SimpleOrthoScaler : MonoBehaviour
{
    [SerializeField] private Vector2 targetAspectRatio;

    private Camera _cam;
    private float _initialSize;
    private float _targetAspectValue;

    void Start()
    {
        _cam = GetComponent<Camera>();

        _initialSize = _cam.orthographicSize;

        _targetAspectValue = targetAspectRatio.x / targetAspectRatio.y;

        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect < _targetAspectValue)
        {
            _cam.orthographicSize = _initialSize * (_targetAspectValue / currentAspect);
        }
        else
        {
            _cam.orthographicSize = _initialSize;
        }
    }
}