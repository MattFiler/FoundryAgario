// Name this script "EffectRadiusEditor"
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DragAlongPath))]
public class DragAlongPathEditor : Editor
{
    public void OnSceneGUI()
    {
        DragAlongPath t = (target as DragAlongPath);
        EditorGUI.BeginChangeCheck();

        Vector3[] points = new Vector3[t.points.Length + 1];
        for (int i = 0; i < t.points.Length; i++)
        {
            points[i] = Handles.FreeMoveHandle(t.points[i], Quaternion.identity, 0.1f, Vector3.one, Handles.CircleHandleCap);
            if (i > 0 && i < t.points.Length && t.points.Length > 1)
            {
                Handles.DrawLine(points[i], points[i - 1]);
            }
        }
        if(t.pathIsLoop)
            Handles.DrawLine(t.points[t.points.Length-1], t.points[0]);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Path Settings");

            for (int i = 0; i < t.points.Length; i++)
            {
                t.points[i] = points[i];
            }
        }
    }
}