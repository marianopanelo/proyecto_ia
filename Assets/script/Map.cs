using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance {  get; private set; }

    [SerializeField] private float width = 30f;
    [SerializeField] private float height = 30f;
    [SerializeField] private bool drawgizmos = true;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Vector3 OutOfMap(Vector3 position)
    {
        Vector3 newPosition = Vector3.zero;
        if (position.x > width / 2f) newPosition.x = -width / 2f;
        if (position.x < -width / 2f) newPosition.x = width / 2f;
        if (position.z > height / 2f) newPosition.z = -height / 2f;
        if (position.z < -height / 2f) newPosition.z = height / 2f;
        return newPosition;
    }

    private void OnDrawGizmos()
    {
        if (!drawgizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 0f, height));
    }



}
