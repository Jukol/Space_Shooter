using UnityEngine;

public class ShowInScene2D : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public float gizmoRadius = 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        DrawWireCircle(transform.position, gizmoRadius);
    }

    void DrawWireCircle(Vector3 position, float radius)
    {
        int segments = 32;  
        float angleStep = 360f / segments;

        Vector3 previousPoint = position + new Vector3(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            Vector3 nextPoint = position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }
}
