
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(JumpData))]
public class JumpDataEditorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        JumpData j = (JumpData)target;
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Gravity");
        var gravity = -(2 * j.jumpHeight) / Mathf.Pow(j.timeToJumpApex, 2);
        GUILayout.Label(gravity.ToString());
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Jump Velocity");
        var jumpVelocity = Mathf.Abs(gravity) * j.timeToJumpApex;
        GUILayout.Label(jumpVelocity.ToString());
        GUILayout.EndHorizontal();
    }
}