#pragma warning disable IDE0044 // readonly modifier hides the serialized field in Unity inspector

using UdonSharp;
using UnityEngine;
using Varneon.VUdon.Editors;
using VRC.SDKBase;

namespace Varneon.VUdon.SimplePlayerSettings
{
    /// <summary>
    /// More advanced but still extremely simple replacement for the original VRCSDK's "VRCWorldSettings" and "AvatarScalingSettings" UdonBehaviours
    /// </summary>
    [AddComponentMenu("VUdon/Simple Player Settings")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SimplePlayerSettings : UdonSharpBehaviour
    {
        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-forces#getwalkspeed--setwalkspeed"/>
        /// </summary>
        [FoldoutHeader("Player Movement")]
        [SerializeField]
        [Tooltip("Working range around 0 - 5\n\nThe speed at which a Player can walk around your world. Set this below your Run Speed.")]
        private float walkSpeed = 2f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-forces#getrunspeed--setrunspeed"/>
        /// </summary>
        [SerializeField]
        [Tooltip("Working range around 0 - 10\n\nThe speed at which a Player can run around your world. Set this above your Walk Speed.")]
        private float runSpeed = 4f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-forces#getstrafespeed--setstrafespeed"/>
        /// </summary>
        [SerializeField]
        [Tooltip("Working range around 0 - 5\n\nThe speed at which a Player can move sideways through your world. Recommended to be set to the same as Walk Speed. Not affected by running.")]
        private float strafeSpeed = 2f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-forces#getjumpimpulse--setjumpimpulse"/>
        /// </summary>
        [SerializeField]
        [Tooltip("Working range around 0 - 10\n\nHow much force applied when a player jumps. Default is 0, so set this if you want to enable jump in your world.")]
        private float jumpImpulse = 3f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-forces#getgravitystrength--setgravitystrength"/>
        /// </summary>
        [SerializeField]
        [Tooltip("Working range around 0 - 10\n\nMultiplier for the Gravity force of the world (set to Earth's default). Don't change Unity's Physics.Gravity in your project, get and set it here instead.")]
        private float gravityStrength = 1f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#set-voice-gain"/>
        /// </summary>
        [FoldoutHeader("Player Voice")]
        [SerializeField]
        [Range(0f, 24f)]
        [Tooltip("In Decibels, Range 0 - 24\n\nAdd boost to the Player's voice in decibels.")]
        private float voiceGain = 15f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#set-voice-distance-near"/>
        /// </summary>
        [SerializeField]
        [Range(0f, 1000000f)]
        [Tooltip("In Meters, Range 0 - 1,000,000\n\nThe near radius, in meters, where volume begins to fall off. It is strongly recommended to leave the Near value at zero for realism and effective spatialization for user voices.")]
        private float voiceDistanceNear = 0f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#set-voice-distance-far"/>
        /// </summary>
        [SerializeField]
        [Range(0f, 1000000f)]
        [Tooltip("In Meters, Range is 0 - 1,000,000\n\nThis sets the end of the range for hearing the user's voice. You can lower this to make another player's voice not travel as far, all the way to 0 to effectively 'mute' the player.")]
        private float voiceDistanceFar = 25f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#set-voice-volumetric-radius"/>
        /// </summary>
        [SerializeField]
        [Range(0f, 1000f)]
        [Tooltip("In Meters, Range is 0 - 1,000\n\nA player's voice is normally simulated to be a point source, however changing this value allows the source to appear to come from a larger area. This should be used carefully, and is mainly for distant audio sources that need to sound \"large\" as you move past them. Keep this at zero unless you know what you're doing. The value for Volumetric Radius should always be lower than Voice Distance Far.\n\nIf you want a user's voice to sound like it is close no matter how far it is, increase the Voice Distance Near range to a large value.")]
        private float voiceVolumetricRadius = 0;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#set-voice-lowpass"/>
        /// </summary>
        [SerializeField]
        [Tooltip("When a voice is some distance off, it is passed through a low-pass filter to help with understanding noisy worlds. You can disable this if you want to skip this filter. For example, if you intend for a player to use their voice channel to play a high-quality DJ mix, turning this filter off is advisable.")]
        private bool voiceLowpass = true;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#setavataraudiogain"/>
        /// </summary>
        [FoldoutHeader("Avatar Audio")]
        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("In Decibels, Range 0-10\n\nSet the Maximum Gain allowed on Avatar Audio.")]
        private float avatarAudioGain = 10f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#setavataraudionearradius"/>
        /// </summary>
        [SerializeField]
        [Tooltip("In Meters, Range is not limited\n\nThis sets the maximum start of the range for hearing the avatar's audio. You can lower this to make another player's avatar not travel as far, all the way to 0 to effectively 'mute' the player. Note that this is compared to the audio source's minDistance, and the smaller value is used.")]
        private float avatarAudioNearRadius = 40f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#setavataraudiofarradius"/>
        /// </summary>
        [SerializeField]
        [Tooltip("In Meters, Range is not limited\n\nThis sets the maximum end of the range for hearing the avatar's audio. You can lower this to make another player's avatar not travel as far, all the way to 0 to effectively 'mute' the player. Note that this is compared to the audio source's maxDistance, and the smaller value is used.")]
        private float avatarAudioFarRadius = 40f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#setavataraudiovolumetricradius"/>
        /// </summary>
        [SerializeField]
        [Tooltip("In Meters, Range is not limited\n\nAn avatar's audio source is normally simulated to be a point source, however changing this value allows the source to appear to come from a larger area. This should be used carefully, and is mainly for distant audio sources that need to sound \"large\" as you move past them. Keep this at zero unless you know what you're doing. The value for Volumetric Radius should always be lower than Avatar AUdio Far Radius.")]
        private float avatarAudioVolumetricRadius = 0;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#setavataraudioforcespatial"/>
        /// </summary>
        [SerializeField]
        [Tooltip("If this is on, then Spatialization is enabled for the source, and the spatialBlend is set to 1.")]
        private bool avatarAudioForceSpatial = false;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-audio#setavataraudiocustomcurve"/>
        /// </summary>
        [SerializeField]
        [Tooltip("This sets whether the audio source should use a pre-configured custom curve.")]
        private bool avatarAudioCustomCurve = false;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-avatar-scaling#setmanualavatarscalingallowed"/>
        /// </summary>
        [FoldoutHeader("Avatar Scaling")]
        [SerializeField]
        [Tooltip("Should players be allowed to manually scale their avatars.")]
        private bool allowManualAvatarScaling = true;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-avatar-scaling#setavatareyeheightminimumbymeters"/>
        /// </summary>
        [SerializeField]
        [Range(0.2f, 5f)]
        [Tooltip("Minimum height in meters that the local player is permitted to scale themselves to in the player-controlled avatar scaling mode. (Must be greater than or equal to 0.2 meters.)")]
        private float minimumHeight = 0.2f;

        /// <summary>
        /// <see href="https://creators.vrchat.com/worlds/udon/players/player-avatar-scaling#setavatareyeheightmaximumbymeters"/>
        /// </summary>
        [SerializeField]
        [Range(0.2f, 5f)]
        [Tooltip("Maximum eye height in meters that the local player is permitted to scale themselves to in the player-controlled avatar scaling mode. (Must be less or equal to 5 meters.)")]
        private float maximumHeight = 5f;

        /// <summary>
        /// Should SimplePlayerSettings intercept avatar height change events to ensure that height is within the allowed range.
        /// </summary>
        [SerializeField]
        [Tooltip("Ensure that player's height is always within the allowed range when the height changes.")]
        private bool alwaysEnforceHeight = false;

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (player.isLocal)
            {
                player.SetWalkSpeed(walkSpeed);
                player.SetRunSpeed(runSpeed);
                player.SetStrafeSpeed(strafeSpeed);
                player.SetJumpImpulse(jumpImpulse);
                player.SetGravityStrength(gravityStrength);

                player.SetManualAvatarScalingAllowed(allowManualAvatarScaling);
                player.SetAvatarEyeHeightMinimumByMeters(minimumHeight);
                player.SetAvatarEyeHeightMaximumByMeters(maximumHeight);
            }
            else
            {
                player.SetVoiceGain(voiceGain);
                player.SetVoiceDistanceNear(voiceDistanceNear);
                player.SetVoiceDistanceFar(voiceDistanceFar);
                player.SetVoiceVolumetricRadius(voiceVolumetricRadius);
                player.SetVoiceLowpass(voiceLowpass);

                player.SetAvatarAudioGain(avatarAudioGain);
                player.SetAvatarAudioNearRadius(avatarAudioNearRadius);
                player.SetAvatarAudioFarRadius(avatarAudioFarRadius);
                player.SetAvatarAudioVolumetricRadius(avatarAudioVolumetricRadius);
                player.SetAvatarAudioForceSpatial(avatarAudioForceSpatial);
                player.SetAvatarAudioCustomCurve(avatarAudioCustomCurve);
            }
        }

        public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
        {
            if (player.isLocal && alwaysEnforceHeight)
            {
                float currentHeight = player.GetAvatarEyeHeightAsMeters();

                float clampedHeight = Mathf.Clamp(currentHeight, minimumHeight, maximumHeight);

                if(clampedHeight != currentHeight)
                {
                    player.SetAvatarEyeHeightByMeters(clampedHeight);
                }
            }
        }
    }
}
