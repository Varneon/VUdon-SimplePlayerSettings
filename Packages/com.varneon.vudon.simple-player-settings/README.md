# VUdon - SimplePlayerSettings

This is a simple, yet more advanced replacement for the original VRCWorldSettings and AvatarScalingSettings UdonBehaviours that ship with VRCSDK that allows you to fully control the default player properties such as forces, voices, avatar audio and scaling.

Benefits of using SimplePlayerSettings:
- Clean and categorized interface with full documentation
- Player properties which the SDK programs don't give access to by default have been included
- All properties previously separated into multiple UdonBehaviours are now combined into one
- Numeric fields have been constrained according to the official documentation to prevent invalid configuration
- SyncMode of the UdonBehaviour has been explicitly set to None to prevent any additional network load

## How to set up SimplePlayerSettings

1) Add SimplePlayerSettings into the scene
    a) Drag & drop the SimplePlayerSettings prefab from "Packages/com.varneon.vudon.simple-player-settings/Runtime/Prefabs/SimplePlayerSettings.prefab" into the scene
    b) Click the "Add Component" button on the Inspector when inspecting a GameObject to which you want to add SimplePlayerSettings and select "VUdon/SimplePlayerSettings"
2) Follow instructions on the inspector about removing conflicting UdonBehaviour(s) (if visible)
3) Configure the player settings as you wish
4) Default player settings should now be applied to everyone who enter the world

## Troubleshooting

- Make sure none of the GameObjects in the same hierarchy which SimplePlayerSettings is attached to is tagged as "EditorOnly"
- Make sure no other UdonBehaviour in the scene applies player settings you want to be set by SimplePlayerSettings
