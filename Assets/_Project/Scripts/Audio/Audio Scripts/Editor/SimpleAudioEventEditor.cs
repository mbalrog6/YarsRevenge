
#if UNITY_EDITOR
using UnityEditor;


using UnityEngine;

   [CustomEditor(typeof(SimpleAudioEvent))]
   public class SimpleAudioEventEditor : Editor
   {
      private AudioSource _previewSource;

      public void OnEnable()
      {
         var audioObject = EditorUtility.CreateGameObjectWithHideFlags(
            "Audio Preview", 
            HideFlags.HideAndDontSave,
            typeof(AudioSource));

         _previewSource = audioObject.GetComponent<AudioSource>();
      }

      private void OnDisable()
      {
         DestroyImmediate(_previewSource.gameObject);
      }

      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();
      
         EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
         if (GUILayout.Button("Preview"))
         {
            var simpleTarget = (SimpleAudioEvent) target;
            simpleTarget.Play(_previewSource);
         }
      
         EditorGUI.EndDisabledGroup();
      }
   }
#endif