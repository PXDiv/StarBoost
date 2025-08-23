using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleParallax))]
public class SimpleParallaxEditor : Editor
{
    bool previewing = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SimpleParallax parallax = (SimpleParallax)target;

        GUILayout.Space(10);

        if (!previewing)
        {
            if (GUILayout.Button("Preview Parallax"))
            {
                if (parallax.cam == null && Camera.main != null)
                    parallax.cam = Camera.main.transform;

                parallax.startPos = parallax.transform.position;
                parallax.ApplyParallax();
                previewing = true;
            }
        }
        else
        {
            if (GUILayout.Button("Stop Preview"))
            {
                parallax.ResetPosition();
                previewing = false;
            }
        }
    }
}
