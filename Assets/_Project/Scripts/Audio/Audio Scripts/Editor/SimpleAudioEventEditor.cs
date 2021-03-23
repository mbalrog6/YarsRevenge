using UnityEditor;
using UnityEngine;

namespace YarsRevenge._Project.Scripts.Audio.Audio_Scripts.Editor
{
   [CustomEditor(typeof(SimpleAudioEvent))]
   public class SimpleAudioEventEditor : UnityEditor.Editor
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
}
