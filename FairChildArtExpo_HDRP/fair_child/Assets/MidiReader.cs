using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.MusicTheory;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEngine.Rendering.HighDefinition;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;
using System.Threading.Tasks;

public class MidiReader : MonoBehaviour
{
    public MidiFile mid;
    private const string OutputDeviceName = "Microsoft GS Wavetable Synth";

    private OutputDevice _outputDevice;

    public MidiClockSettings ClockSettings { get; private set; }

    private Playback _playback;

    public notePair[] musicNotes;
    //get Note readable by engine

    public delegate void publishNote(NoteName note, bool onOff);
    public publishNote dg_publish;
    

    public void Start()
    {
        mid = MidiFile.Read(Application.streamingAssetsPath + "/audio/TestMIDI.mid");
        
        InitializeOutputDevice();
        Play();
        Go();
        
    }
    private void InitializeOutputDevice()
    {
        Debug.Log($"Initializing output device [{OutputDeviceName}]...");

        var allOutputDevices = OutputDevice.GetAll();
        if (!allOutputDevices.Any(d => d.Name == OutputDeviceName))
        {
            var allDevicesList = string.Join(Environment.NewLine, allOutputDevices.Select(d => $"  {d.Name}"));
            Debug.Log($"There is no [{OutputDeviceName}] device presented in the system. Here the list of all device:{Environment.NewLine}{allDevicesList}");
            return;
        }

        _outputDevice = OutputDevice.GetByName(OutputDeviceName);
        Debug.Log($"Output device [{OutputDeviceName}] initialized.");
    }
    public void Play()
    {
        ClockSettings = new MidiClockSettings
        {
            CreateTickGeneratorCallback = () => new RegularPrecisionTickGenerator()
        };
        PlaybackSettings settings = new PlaybackSettings();
        settings.ClockSettings = ClockSettings;
        _playback = mid.GetPlayback(_outputDevice, settings);
        _playback.Loop = true;
        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        _playback.NotesPlaybackFinished += OnNotesPlaybackFinished;
        
        
    }

    public void Go()
    {
        _playback.Start();
    }
    private void OnApplicationQuit()
    {
        Debug.Log("Releasing playback and device...");

        if (_playback != null)
        {
            _playback.NotesPlaybackStarted -= OnNotesPlaybackStarted;
            _playback.NotesPlaybackFinished -= OnNotesPlaybackFinished;
            _playback.Dispose();
        }

        if (_outputDevice != null)
            _outputDevice.Dispose();

        Debug.Log("Playback and device released.");
    }

    private void OnNotesPlaybackFinished(object sender, NotesEventArgs e)
    {
        LogNotes("Notes finished:", e, false);
        goAway(e, false);
    }

   

    private void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
    {
        LogNotes("Notes started:", e, true);

        goAway(e, true);
    }

    private void LogNotes(string title, NotesEventArgs e, bool start)
    {
        var message = new StringBuilder()
            .AppendLine(title)
            .AppendLine(string.Join(Environment.NewLine, e.Notes.Select(n => $"  {n}")))
            .ToString();
        Debug.Log(message.Trim());
       
    }

    public void goAway(NotesEventArgs e, bool start)
    {
        string read = e.Notes.Select(n => $"{n}").Last();

        NoteName n = NoteName.F;
        switch (read[0])
        {
            case 'C':
                if (read[1] == '#')
                    n = NoteName.CSharp;
                else
                    n = NoteName.C;
                break;

            case 'D':
                if ( read[1] == '#')
                    n = NoteName.DSharp;
                else
                    n = NoteName.D;
                break;

            case 'E':
                n = NoteName.E;
                break;

            case 'F':
                if ( read[1] == '#')
                    n = NoteName.FSharp;
                else
                    n = NoteName.F;
                break;

            case 'G':
                if (read[1] == '#')
                    n = NoteName.GSharp;
                else
                    n = NoteName.G;
                break;

            case 'A':
                if (read[1] == '#')
                    n = NoteName.ASharp;
                else
                    n = NoteName.A;
                break;

            case 'B':
                n = NoteName.B;
                break;
            default:
                Debug.Log("FAW FAW" + read);
                break;
        }
        Debug.Log("GAW GAW" + n.ToSafeString());
        //
        Alight(n, start);
        //dg_publish.Invoke(n, true);

    }

    public async void Alight(NoteName n, bool b)
    {
        MidiThreadListener.Instance.AddJob(() =>
        {
            MidiThreadListener.Instance.Listener(n, b);
        });

        await Task.Delay(0);
    }
}
[System.Serializable]
public class notePair
{
    public GameObject gobject;
    public NoteName ne;
}

