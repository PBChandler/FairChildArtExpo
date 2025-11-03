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
    public string file;
    private OutputDevice _outputDevice;

    public float delay = 0.1f;

    public MidiClockSettings ClockSettings { get; private set; }

    private Playback _playback;

    public notePair[] musicNotes;
    //get Note readable by engine

    public delegate void publishNote(NoteName note, bool onOff);
    public publishNote dg_publish;
    

    public void Start()
    {
        mid = MidiFile.Read(Application.streamingAssetsPath + "/audio/"+file+".mid");
        
        InitializeOutputDevice();
       
        Invoke("Go", 0.5f);
        
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
        _playback.Loop = true; //determine if track is looping
        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        _playback.NotesPlaybackFinished += OnNotesPlaybackFinished;
        
        
    }

    public void Go()
    {
        Play();
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
        
        //
        Alight(read, start);
        //dg_publish.Invoke(n, true);

    }

    public async void Alight(string n, bool b)
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
    public string tuning;
}

