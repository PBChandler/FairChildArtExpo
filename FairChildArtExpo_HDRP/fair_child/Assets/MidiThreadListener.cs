using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MidiThreadListener : MonoBehaviour
{
    public MidiReader publisher;
    public static MidiThreadListener Instance;
    Queue<Action> jobs = new Queue<Action>();

    public notePair[] musicNotes;
    void Start()
    {
        Instance = this;
        //publisher.dg_publish += Listener;
    }
    void Update()
    {
        while (jobs.Count > 0)
            jobs.Dequeue().Invoke();
    }

    internal void AddJob(Action newJob)
    {
        jobs.Enqueue(newJob);
    }


    public void Listener(NoteName n, bool state)
    {
        musicNotes.First(p => p.ne == n).gobject.SetActive(state);
    }
   
}
