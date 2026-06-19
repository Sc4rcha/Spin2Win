using UnityEditor;
using UnityEngine;

public class Map : MonoBehaviour
{
    public float Radius;
    public Vector2 Centre;
    public float StrengthCentre;
    public float StrengthCollision;

    public bool CheckCollision(Transform spinner) 
    {
        if (Vector2.Distance(spinner.position, transform.position) > Radius)
        {
            spinner.position = transform.position + (spinner.position - transform.position).normalized * Radius;
            return true;
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, Radius);
        Handles.color = Color.yellow;
        Handles.DrawLine(Centre + Vector2.up / 2, Centre + Vector2.down / 2);
        Handles.DrawLine(Centre + Vector2.left / 2, Centre + Vector2.right / 2);
    }
#endif
}
