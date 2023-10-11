using System.Linq;
using UnityEditor;
using UnityEngine;
using Varneon.VUdon.Editors.Editor;
using VRC.Udon;

namespace Varneon.VUdon.SimplePlayerSettings.Editor
{
    [CustomEditor(typeof(SimplePlayerSettings))]
    public class SimplePlayerSettingsEditor : InspectorBase
    {
        [SerializeField]
        private Texture2D headerIcon;

        private const string VRCWORLDSETTINGS_PROGRAM_GUID = "c8df303ceb45ae84f85a11591f741734";

        private const string VRCWORLDSETTINGS_PROGRAM_NAME = "VRCWorldSettings";

        private UdonBehaviour[] vrcWorldSettingsBehaviours;

        private bool hasVRCWorldSettingsBehaviours;

        protected override string PersistenceKey => "Varneon/VUdon/Logger/UdonConsole/Editor/Foldouts";

        protected override InspectorHeader Header => new InspectorHeaderBuilder("VUdon - Simple Player Settings", "Simplified controls of default player settings for worlds.")
            .WithIcon(headerIcon)
            .WithURL("GitHub", "https://github.com/Varneon/VUdon-SimplePlayerSettings")
            .Build();

        protected override void OnEnable()
        {
            base.OnEnable();

            vrcWorldSettingsBehaviours = FindObjectsOfType<UdonBehaviour>().Where(u => IsUdonBehaviourVRCWorldSettings(u)).ToArray();

            hasVRCWorldSettingsBehaviours = vrcWorldSettingsBehaviours.Length > 0;
        }

        protected override void OnPreDrawFields()
        {
            if (hasVRCWorldSettingsBehaviours)
            {
                EditorGUILayout.HelpBox("Your scene has default VRCWorldSettings behaviour(s) in it!\n\nThese will cause conflicts with SimplePlayerSettings.\n\nRemove all VRCWorldSettings behaviours to ensure intented functionality of SimplePlayerSettings.", MessageType.Error);

                if(GUILayout.Button("Remove All VRCWorldSettings Behaviours"))
                {
                    RemoveAllVRCWorldSettingsBehaviours();
                }
            }
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
