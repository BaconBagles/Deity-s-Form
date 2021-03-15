using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompositeBehaviour))]
public class CompositeBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        {
            CompositeBehaviour cb = (CompositeBehaviour)target;

            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Add"))
            {
                AddBehaviour(cb);
                EditorUtility.SetDirty(cb);
            }

            if (GUILayout.Button("Remove"))
            {
                RemoveBehaviour(cb);
                EditorUtility.SetDirty(cb);
            }

            EditorGUILayout.EndVertical();

            if (cb.behaviours == null || cb.behaviours.Length == 0)
            {
                EditorGUILayout.HelpBox("No behaviours in array.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20f);
                EditorGUILayout.LabelField("Number", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.LabelField("Behaviors", GUILayout.MinWidth(60f));
                EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();

                for (int i = 0; i < cb.behaviours.Length; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(15f));
                    cb.behaviours[i] = (EnemyFlockBehaviour)EditorGUILayout.ObjectField(cb.behaviours[i], typeof(EnemyFlockBehaviour), false, GUILayout.MinWidth(60f));
                    cb.weights[i] = EditorGUILayout.FloatField(cb.weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                    EditorGUILayout.EndHorizontal();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(cb);
                }
            }
        }
    }

    void AddBehaviour(CompositeBehaviour cb)
    {
        int oldCount = (cb.behaviours != null) ? cb.behaviours.Length : 0;
        EnemyFlockBehaviour[] newBehaviours = new EnemyFlockBehaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        for( int i = 0; i < oldCount; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        newWeights[oldCount] = 1f;
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }

    void RemoveBehaviour(CompositeBehaviour cb)
    {
        int oldCount = cb.behaviours.Length;
        if(oldCount == 1)
        {
            cb.behaviours = null;
            cb.weights = null;
            return;
        }
        EnemyFlockBehaviour[] newBehaviours = new EnemyFlockBehaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }

}
