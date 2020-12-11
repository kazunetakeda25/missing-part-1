using System;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_FLASH  // AudioClip.GetData not supported by Flash

/// <summary>
/// Auxiliary class to play two Unity AudioClips one after the other without a gap 
/// </summary>
/// <remarks>
/// Is is required that all clips have the same number of channels and sample rate.
/// In addition, clips must be uncompressed in memory.
/// </remarks>
public class GaplessAudioClipTransition
{
    /// <summary>
    /// The <see cref="AudioClip"/> that is currently playing.
    /// </summary>
    /// <remarks>
    /// Set to the <see cref="AudioClip"/> that should be started when playing.
    /// </remarks>
    public AudioClip currentClip
    {
        set
        {
            if ( !object.ReferenceEquals(_currentClip, value ) )
            {
                _currentClip = value;
                _GetClipData( out _currentClipData, _currentClip ); // all data must be retrieved now, as GetData can not be called in callback function
            }
            _positionInCurrentClip = 0;
        }
        get
        {
            return _currentClip;
        }
    }

    /// <summary>
    /// The <see cref="AudioClip"/> that will be played after <see cref="currentClip"/> has finished.
    /// </summary>
    public AudioClip nextClip
    {
        set
        {
            if ( !object.ReferenceEquals( _nextClip, value ) )
            {
                _nextClip = value;

                //Debug.Log( "current: " + _currentClip.name + " next: " + _nextClip.name );

                if ( object.ReferenceEquals(_currentClip, _nextClip ) && !object.ReferenceEquals( _nextClip, null ) )
                {
                    _nextClipData = _currentClipData; // can use same data
                }
                else
                {
                    _GetClipData( out _nextClipData, _nextClip );
                }
            }
        }
        get
        {
            return _currentClip;
        }
    }

    /// <summary>
    /// The <see cref="AudioClip"/> that will output the audio gaplessly.
    /// </summary>
    /// <remarks>
    /// Play an AudioSource with this AudioClip to hear the generated gapless sound.
    /// </remarks>
    public AudioClip outputClip
    {
        get
        {
            return _outputClip;
        }
    }

    /// <summary>
    /// Will be invoked if the playback changes from the <see cref="currentClip"/> to the <see cref="nextClip"/>
    /// </summary>
    public Action onTransitionCallback;

    AudioClip _currentClip;
    AudioClip _nextClip;
    AudioClip _outputClip;

    float[] _currentClipData;
    float[] _nextClipData;

    int _positionInOutputClip;
    int _positionInCurrentClip;

    int _outputChannels;
    int _outputFrequency;

    int _outputSamples;

    bool _outputClipCreated;

    bool _resetCurrentClipPos = false;

    float timeInOutputClip
    {
        get
        {
            return (float)_positionInOutputClip / ( (float)_outputChannels * _outputFrequency );
        }
    }

    /// <summary>
    /// Creates a GaplessAudioClipTransition object.
    /// </summary>
    /// <param name="startClip">The AudioClip to use when starting the playback</param>
    /// <param name="nextClip">The AudioClip that will be playing right after the startClip without a gap.</param>
    public GaplessAudioClipTransition( AudioClip startClip, AudioClip nextClip )
    {
        this.currentClip = startClip;
        this.nextClip = nextClip;
    }

    /// <summary>
    /// Must be called before gapless playing can be started.  
    /// </summary>
    /// <remarks>
    /// You must specify valid <see cref="currentClip"/> and <see cref="nextClip"/> before calling <c>CreateOutputClip</c>.
    /// </remarks>
    /// <param name="channels">The number of channels</param>
    /// <param name="frequency">The frequency in Hz</param>
    /// <param name="use3d">must be <c>true</c> if the AudioClip is used for 3d</param>
    /// <returns>
    /// The created <see cref="AudioClip"/> (<see cref="outputClip"/> will return the same)
    /// </returns>
    public AudioClip CreateOutputClip( int channels, int frequency, bool use3d )
    {
        const float outputClipLengthInSeconds = 15.0f;

        if ( currentClip == null )
        {
            Debug.LogError( "currentClip must be specified before calling CreateOutputClip()" );
        }

        _outputChannels = channels;
        _outputFrequency = frequency;
        _outputSamples = Mathf.RoundToInt( outputClipLengthInSeconds * frequency );

        _outputClipCreated = false;
        _resetCurrentClipPos = true;

        _outputClip = AudioClip.Create( "GaplessTransistionClip", _outputSamples, _outputChannels, _outputFrequency, use3d, true, _OnAudioRead, _OnAudioSetPosition );
        _outputClipCreated = true;

        //Debug.Log( "Created GaplessTransistionClip" );
        return _outputClip;
    }

    void _MyCopyArray( float[] source, int sourceIndex, float[] dest, int destinationIndex, int length )
    {
        //Array.Copy( source, sourceIndex, dest, destinationIndex, length ); // throws error - TODO - check why

        for ( int i = 0; i < length; i++ )
        {
            dest[ destinationIndex + i ] = source[ sourceIndex + i ];
        }
    }

    void _OnAudioRead( float[] data )
    {
        if ( !_outputClipCreated )
        {
            return;
        }

        //float dataToTime = 1.0f / ( (float) _outputFrequency * _outputChannels );

        //Debug.Log( "Seconds read: " + (float)data.Length / ( _outputChannels * _outputFrequency ) );

        int overlap = _positionInCurrentClip + data.Length - _currentClipData.Length;
        if ( overlap <= 0 )
        {
            _MyCopyArray( _currentClipData, _positionInCurrentClip, data, 0, data.Length );
            _positionInCurrentClip += data.Length;
        }
        else
        {           
            int firstPartLength = data.Length - overlap;
            int secondPartLength = Mathf.Min( overlap, _nextClipData.Length );

            _MyCopyArray( _currentClipData, _positionInCurrentClip, data, 0, firstPartLength );
            _MyCopyArray( _nextClipData, 0, data, firstPartLength, secondPartLength );

            _SwapCurrentAndNextClip();

            _positionInCurrentClip = secondPartLength;

            if ( onTransitionCallback != null )
            {
                onTransitionCallback();
            }
        }

        _positionInOutputClip += data.Length;
        if ( _positionInOutputClip >= _outputSamples * _outputChannels )
        {
            _resetCurrentClipPos = false;
        }
    }

    private void _SwapCurrentAndNextClip()
    {
        var clipTmp = _currentClip;
        _currentClip = _nextClip;
        _nextClip = clipTmp;

        var dataTmp = _currentClipData;
        _currentClipData = _nextClipData;
        _nextClipData = dataTmp;
    }

    // is called by AudioCource.Create and AudioSource.Play with position = 0
    // is also called if AudioSource loops, in this case _positionInCurrentClip must stay the same
    void _OnAudioSetPosition( int newPosition ) 
    {
        if ( newPosition != 0 )
        {
            Debug.LogWarning( "AudioSetPosition not supported: " + newPosition );
        }

        if ( _resetCurrentClipPos )
        {
            _positionInCurrentClip = 0;
        }
        _positionInOutputClip = newPosition;
    }

    private void _GetClipData( out float[] clipData, AudioClip clip )
    {
        clipData = new float[ clip.samples * clip.channels ];
        clip.GetData( clipData, 0 );
    }
}
#endif
