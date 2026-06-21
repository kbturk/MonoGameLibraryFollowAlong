using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonogameLibrary.Audio;

public class AudioController : IDisposable
{
    private readonly List<SoundEffectInstance> _activeSoundEffectIntances;

    //Tracks the volume for song playback when muting and unmuting
    private float _previousSongVolume;

    //Tracks the volume for sound effect when muting and unmuting
    private float _previousSoundEffectVolume;

    ///<summary>
    ///Get a value that indicates if audio is muted
    ///</summary>
    public bool IsMuted {get; private set;}

    ///<summary>
    ///Gets or sets the gobal volume of songs.
    ///</summary>
    ///<remarks>
    ///If IsMuted is true, the getter will always return back 0.0f and
    ///the setter will ignore setting the voume.
    ///</remarks>
    public float SongVolume {get { if(IsMuted) return 0.0f; return MediaPlayer.Volume;}
        set { if(IsMuted) return; MediaPlayer.Volume = Math.Clamp(value, 0.0f, 1.0f);}}

    /// <summary>
    /// Gets or Sets the global volume of sound effects.
    /// </summary>
    /// <remarks>
    /// If IsMuted is true, the getter will always return back 0.0f and the
    /// setter will ignore setting the volume.
    /// </remarks>
    public float SoundEffectVolume {get { if(IsMuted) return 0.0f; return SoundEffect.MasterVolume;}
        set { if(IsMuted) return; SoundEffect.MasterVolume = Math.Clamp(value, 0.0f, 1.0f);}}

    ///<summary>
    ///Gets a value that indicates if this audio controller has been disposed.
    ///</summary>
    public bool IsDisposed { get; private set; }

    ///<summary>
    /// Creates a new audio controller instance.
    ///</summary>
    public AudioController()
    {
        _activeSoundEffectIntances = new List<SoundEffectInstance>();
    }

    //Finalizer called when object is collected by the garbage collector.
    ~AudioController() => Dispose(false);

    ///<summary>
    ///Updates this audio controller.
    ///</summary>
    public void Update()
    {
        for (int i = _activeSoundEffectIntances.Count -1; i >= 0; i--)
        {
            SoundEffectInstance instance = _activeSoundEffectIntances[i];

            if (instance.State == SoundState.Stopped)
            {
                if (!instance.IsDisposed)
                {
                    instance.Dispose();
                }
                _activeSoundEffectIntances.RemoveAt(i);
            }
        }
    }

   ///<summary>
   ///Plays the given sound effect.
   ///</summary>
   ///<param name="soundEffect">the sound effect to play.</param>
   ///<returns>The sound effect instance created by this method.</returns>
   public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect)
   {
       return PlaySoundEffect(soundEffect, 1.0f, 0.0f, 0.0f, false);
   }

   ///<summary>
   ///Plays the given sound effect.
   ///</summary>
   ///<param name="soundEffect">the sound effect to play.</param>
   ///<param name="volume">the volume level, ranging from 0.0 (silence) to 1.0 (full vol)</param>
   ///<param name="pitch">the pitch adjustment ranging from -1.0 (down an octave) to 0.0 (no change)</param>
   ///<param name="pan">the panning, ranging from -1 (left speaker) to 0 (center) to 1 (right speaker).</param>
   ///<param name="isLooped">whether or not to loop the sound effect.</param>
   ///<returns>The sound effect instance created by this method.</returns>
   public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan, bool isLooped)
   {
       // create an instance from teh sound effect given
       SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

       //Apply the volume, pitch, pan, and loop values specified.
       soundEffectInstance.Volume = volume;
       soundEffectInstance.Pitch = pitch;
       soundEffectInstance.Pan = pan;
       soundEffectInstance.IsLooped = isLooped;

       // Tell the instance to play
       soundEffectInstance.Play();

       // Add it to the active instances for tracking
       _activeSoundEffectIntances.Add(soundEffectInstance);

       return soundEffectInstance;
   }

   ///<summary>
   ///Plays the given song.
   ///</summary>
   ///<param name="song">The song to play.</param>
   ///<param name="isRepeating">Optionally specify if the song should repeat. Default is true.</param>
   public void PlaySong(Song song, bool isRepeating = true)
   {
       // Check if the media player is already playing. If so, stop it.
       // this is a MUST.
       if (MediaPlayer.State == MediaState.Playing)
           MediaPlayer.Stop();

       MediaPlayer.Play(song);
       MediaPlayer.IsRepeating = isRepeating;
   }

   ///<summary>
   ///Pauses all audio.
   ///</summary>
   public void PauseAudio()
   {
       //Pause any active songs playing.
       MediaPlayer.Pause();

       //Pause any active sound effects.
       foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectIntances)
       {
           soundEffectInstance.Pause();
       }
   }

   /// <summary>
   /// Resumes play of all previous pauded audio.
   /// </summary>
   public void ResumeAudio()
   {
       // Resume paused music.
       MediaPlayer.Resume();

       // Resume any active sound effects.
       foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectIntances)
       {
           soundEffectInstance.Resume();
       }
   }

   ///<summary>
   ///Mutes all audio
   ///</summary>
   public void MuteAudio()
   {
    // Store the volume so they can be restored during UnmuteAudio
    _previousSongVolume = MediaPlayer.Volume;
    _previousSoundEffectVolume = SoundEffect.MasterVolume;

    // Set all volumes to 0
    MediaPlayer.Volume = 0.0f;
    SoundEffect.MasterVolume = 0.0f;

    IsMuted = true;
   }

/// <summary>
/// Unmutes all audio to the volume level prior to muting.
/// </summary>
public void UnmuteAudio()
{
    // Restore the previous volume values.
    MediaPlayer.Volume = _previousSongVolume;
    SoundEffect.MasterVolume = _previousSoundEffectVolume;

    IsMuted = false;
}

/// <summary>
/// Toggles the current audio mute state.
/// </summary>
public void ToggleMute()
{
    if (IsMuted)
    {
        UnmuteAudio();
    }
    else
    {
        MuteAudio();
    }
}

/// <summary>
/// Disposes of this audio controller and cleans up resources.
/// </summary>
public void Dispose()
{
    Dispose(true);
    GC.SuppressFinalize(this);
}

/// <summary>
/// Disposes this audio controller and cleans up resources.
/// </summary>
/// <param name="disposing">Indicates whether managed resources should be disposed.</param>
protected void Dispose(bool disposing)
{
    if(IsDisposed)
    {
        return;
    }

    if (disposing)
    {
        foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectIntances)
        {
            soundEffectInstance.Dispose();
        }
        _activeSoundEffectIntances.Clear();
    }

    IsDisposed = true;
}
}
