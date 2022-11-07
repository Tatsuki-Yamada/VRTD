using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Plugins.NestedAnimator.Editor
{
    public class NestedAnimator : EditorWindow
    {
        public string animatorName;
        public List<string> animationsNames = new List<string>();
        private Vector2 scrollPos;

        [MenuItem("Tools/Nested Animator/Create")]
        private static void OpenWindow()
        {
            GetWindow<NestedAnimator>().Show();
        }

        private void OnGUI()
        {
            AnimatorName();
            Animations();
            Save();
            Repaint();
        }

        private void AnimatorName()
        {
            animatorName = EditorGUILayout.TextField("Animator name: ", animatorName);
        }

        private void Animations()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            var serialized = new SerializedObject(this);
            var properties = serialized.FindProperty("animationsNames");
            EditorGUILayout.PropertyField(properties, true);
            serialized.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();
        }

        private void Save()
        {
            if (!GUILayout.Button("Save")) return;
            if (string.IsNullOrEmpty(animatorName))
            {
                EditorUtility.DisplayDialog("Error", "Animator name cannot be empty!", "Ok");
                return;
            }

            animationsNames.RemoveAll(string.IsNullOrEmpty);

            if (animationsNames.Count == 0)
            {
                EditorUtility.DisplayDialog("Error", "Amount of nested animations cannot be zero!", "Ok");
                return;
            }

            var previousPath = PlayerPrefs.HasKey(ToString())
                ? PlayerPrefs.GetString(ToString())
                : Application.dataPath;

            var path = EditorUtility.SaveFilePanel("Save dialog", previousPath, animatorName, "controller");
            if (path.Length == 0) return;

            PlayerPrefs.SetString(ToString(), path);
            path = "Assets" + path.Substring(Application.dataPath.Length);

            var animator = AnimatorController.CreateAnimatorControllerAtPath(path);

            foreach (var animationClip in animationsNames.Select(animation => new AnimationClip { name = animation }))
            {
                AssetDatabase.AddObjectToAsset(animationClip, animator);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));
            }

            EditorUtility.DisplayDialog("Success!", "Nested animator was created!", "Ok");
        }
    }
}
