using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBounder : MonoBehaviour {

    [SerializeField] private float _minSize = 10;
    [SerializeField] private float _buffer = 2;
    [SerializeField] private Transform playerPlanetStore;
    [SerializeField] private CinemachineVirtualCamera _cineCam;
    private Camera _cam;

    // Start is called before the first frame update
    void Start() {
        _cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        (var center, var size) = CalculateOrthoSize();
        _cam.transform.position = center;
        float newSize = size > _minSize ? size : _minSize;
        float oldSize = _cineCam.m_Lens.OrthographicSize;
        _cineCam.m_Lens.OrthographicSize = Mathf.Lerp(oldSize, newSize, 0.08f);
    }

    private (Vector3 center, float size) CalculateOrthoSize(){
        Bounds bounds = new Bounds(playerPlanetStore.position, Vector3.zero);

        // Give all stuck planets colliders here
        foreach(var col in playerPlanetStore.GetComponentsInChildren<CircleCollider2D>()) bounds.Encapsulate(col.bounds);

        bounds.Expand(_buffer);

        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

        var size = Mathf.Max(horizontal, vertical) * 0.5f;
        var center = bounds.center;
        center.z = -10;

        return (center, size);
    }
}
