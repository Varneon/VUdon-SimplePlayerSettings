using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.Udon;

namespace Varneon.VUdon.SimplePlayerSettings.Editor
{
    [CustomEditor(typeof(SimplePlayerSettings))]
    public class SimplePlayerSettingsEditor : UnityEditor.Editor
    {
        private const string VRCWORLDSETTINGS_PROGRAM_GUID = "c8df303ceb45ae84f85a11591f741734";

        private const string VRCWORLDSETTINGS_PROGRAM_NAME = "VRCWorldSettings";

        private UdonBehaviour[] vrcWorldSettingsBehaviours;

        private bool hasVRCWorldSettingsBehaviours;

        private void OnEnable()
        {
            vrcWorldSettingsBehaviours = FindObjectsOfType<UdonBehaviour>().Where(u => IsUdonBehaviourVRCWorldSettings(u)).ToArray();

            hasVRCWorldSettingsBehaviours = vrcWorldSettingsBehaviours.Length > 0;
        }

        public override void OnInspectorGUI()
        {
            if (hasVRCWorldSettingsBehaviours)
            {
                EditorGUILayout.HelpBox("Your scene has default VRCWorldSettings behaviour(s) in it!\n\nThese will cause conflicts with SimplePlayerSettings.\n\nRemove all VRCWorldSettings behaviours to ensure intented functionality of SimplePlayerSettings.", MessageType.Error);

                if(GUILayout.Button("Remove All VRCWorldSettings Behaviours"))
                {
                    RemoveAllVRCWorldSettingsBehaviours();
                }
            }

            base.OnInspectorGUI();
        }

        private static bool IsUdonBehaviourVRCWorldSettings(UdonBehaviour udonBehaviour)
        {
            return udonBehaviour.programSource.name == VRCWORLDSETTINGS_PROGRAM_NAME && AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(udonBehaviour.programSource)) == VRCWORLDSETTINGS_PROGRAM_GUID;
        }

        private void RemoveAllVRCWorldSettingsBehaviours()
        {
            foreach(UdonBehaviour udonBehaviour in vrcWorldSettingsBehaviours)
            {
                Debug.Log($"Removing VRCWorldSettings UdonBehaviour from {udonBehaviour.gameObject.name}...", udonBehaviour.gameObject);

                Undo.DestroyObjectImmediate(udonBehaviour);
            }

            hasVRCWorldSettingsBehaviours = false;
        }
    }
}
