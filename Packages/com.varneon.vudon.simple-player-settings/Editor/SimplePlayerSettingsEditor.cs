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

        private const string AVATARSCALINGSETTINGS_PROGRAM_GUID = "566cc00e27d5822449529a3785eae366";

        private const string AVATARSCALINGSETTINGS_PROGRAM_NAME = "AvatarScalingSettings";

        private UdonBehaviour[] vrcWorldSettingsBehaviours;

        private UdonBehaviour[] avatarScalingSettingsBehaviours;

        private bool hasVRCWorldSettingsBehaviours;

        private bool hasAvatarScalingSettingsBehaviours;

        protected override string PersistenceKey => "Varneon/VUdon/SimplePlayerSettings/Editor/Foldouts";

        protected override InspectorHeader Header => new InspectorHeaderBuilder("VUdon - Simple Player Settings", "Simplified controls of default player settings for worlds.")
            .WithIcon(headerIcon)
            .WithURL("GitHub", "https://github.com/Varneon/VUdon-SimplePlayerSettings")
            .Build();

        protected override void OnEnable()
        {
            base.OnEnable();

            vrcWorldSettingsBehaviours = FindObjectsOfType<UdonBehaviour>().Where(u => IsUdonBehaviourVRCWorldSettings(u)).ToArray();

            avatarScalingSettingsBehaviours = FindObjectsOfType<UdonBehaviour>().Where(u => IsUdonBehaviourAvatarScalingSettings(u)).ToArray();

            hasVRCWorldSettingsBehaviours = vrcWorldSettingsBehaviours.Length > 0;

            hasAvatarScalingSettingsBehaviours = avatarScalingSettingsBehaviours.Length > 0;
        }

        protected override void OnPreDrawFields()
        {
            if (hasVRCWorldSettingsBehaviours)
            {
                EditorGUILayout.HelpBox("Your scene has default VRCWorldSettings behaviour(s) in it!\n\nThese will cause conflicts with SimplePlayerSettings.\n\nRemove all VRCWorldSettings behaviours to ensure intended functionality of SimplePlayerSettings.", MessageType.Error);

                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Select VRCWorldSettings"))
                    {
                        Selection.objects = vrcWorldSettingsBehaviours.Select(b => b.gameObject).ToArray();
                    }
                    else if (GUILayout.Button("Remove VRCWorldSettings"))
                    {
                        RemoveAllVRCWorldSettings();
                    }
                }
            }

            if (hasAvatarScalingSettingsBehaviours)
            {
                EditorGUILayout.HelpBox("Your scene has default AvatarScalingSettings behaviour(s) in it!\n\nThese will cause conflicts with SimplePlayerSettings.\n\nRemove all AvatarScalingSettings behaviours to ensure intended functionality of SimplePlayerSettings.", MessageType.Error);

                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Select AvatarScalingSettings"))
                    {
                        Selection.objects = avatarScalingSettingsBehaviours.Select(b => b.gameObject).ToArray();
                    }
                    else if (GUILayout.Button("Remove AvatarScalingSettings"))
                    {
                        RemoveAllAvatarScalingSettings();
                    }
                }
            }
        }

        private static bool IsUdonBehaviourVRCWorldSettings(UdonBehaviour udonBehaviour)
        {
            return udonBehaviour.programSource.name == VRCWORLDSETTINGS_PROGRAM_NAME && AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(udonBehaviour.programSource)) == VRCWORLDSETTINGS_PROGRAM_GUID;
        }

        private static bool IsUdonBehaviourAvatarScalingSettings(UdonBehaviour udonBehaviour)
        {
            return udonBehaviour.programSource.name == AVATARSCALINGSETTINGS_PROGRAM_NAME && AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(udonBehaviour.programSource)) == AVATARSCALINGSETTINGS_PROGRAM_GUID;
        }

        private void RemoveAllVRCWorldSettings()
        {
            foreach(UdonBehaviour udonBehaviour in vrcWorldSettingsBehaviours)
            {
                if (udonBehaviour == null) { continue; }

                Debug.Log($"Removing VRCWorldSettings UdonBehaviour from {udonBehaviour.gameObject.name}...", udonBehaviour.gameObject);

                Undo.DestroyObjectImmediate(udonBehaviour);
            }

            hasVRCWorldSettingsBehaviours = false;
        }

        private void RemoveAllAvatarScalingSettings()
        {
            foreach (UdonBehaviour udonBehaviour in avatarScalingSettingsBehaviours)
            {
                if(udonBehaviour == null) { continue; }

                Debug.Log($"Removing AvatarScalingSettings UdonBehaviour from {udonBehaviour.gameObject.name}...", udonBehaviour.gameObject);

                Undo.DestroyObjectImmediate(udonBehaviour);
            }

            hasAvatarScalingSettingsBehaviours = false;
        }
    }
}
