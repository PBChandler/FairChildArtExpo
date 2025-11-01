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

    public Vector3 eulers;
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


    public void Listener(string n, bool state)
    {
        if(state)
            musicNotes.First(p => p.tuning == n).gobject.transform.Rotate(eulers);
        else
            musicNotes.First(p => p.tuning == n).gobject.transform.Rotate(-eulers);
    }
   
}
