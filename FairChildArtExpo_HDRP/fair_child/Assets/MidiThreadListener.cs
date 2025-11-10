using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MidiThreadListener : MonoBehaviour
{
    public MidiReader publisher;
    public static MidiThreadListener Instance;
    Queue<Action> jobs = new Queue<Action>();

    public Vector3 eulers, defaultEulers;
    public notePair[] musicNotes;

    public bool generateNewTimeStamps = false;
    public List<TimeStamp> timeStamps = new List<TimeStamp>();

    [System.Serializable]
    public struct TimeStamp
    {
        public float time;
        public GameObject gameObject;
        public bool on;
    }

    public int index = 0, secondIndex = 0;
    void Start()
    {
        Instance = this;
        defaultEulers = musicNotes[0].gobject.transform.eulerAngles;
        //publisher.dg_publish += Listener;
        if (timeStamps.Count > 0)
        {
            foreach (var t in timeStamps)
            {
                StartCoroutine(PlayTimeStamp(t));
                index++;
            }
        }
    }

    public IEnumerator PlayTimeStamp(TimeStamp t)
    {
        bool complete = false;
        yield return new WaitForSeconds(t.time-0.1f-0.5f);

        if (t.on)
        {

            StartCoroutine(RotateForward(t.gameObject.transform, t.gameObject.transform.rotation, Quaternion.Euler(eulers), 0.1f, 0.1f));
            complete = true;
        }
        else
        {

            StartCoroutine(RotateForward(t.gameObject.transform, t.gameObject.transform.rotation, Quaternion.Euler(-eulers), 0.2f, 0.1f));
            complete = true;
        }
    }

    public IEnumerator RotateForward(Transform f, Quaternion start, Quaternion goal, float endergoal, float enderspeed)
    {
        float time = 0;

        while (time < endergoal)
        {
            f.rotation = Quaternion.Lerp(f.rotation, goal, enderspeed);
            yield return new WaitForSeconds(0.01f);
        }
        f.rotation = goal;
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
        Transform g;
        try
        {
            g = musicNotes.First(p => p.tuning == n).gobject.transform;
        }
        catch
        {
            Debug.LogWarning(n);
            g = musicNotes.First(p => p.tuning == n).gobject.transform;
        }
        
        //if (state)
        //    g.Rotate(eulers);
        //else
        //    g.Rotate(-eulers);


        if (generateNewTimeStamps)
        {
            TimeStamp t = new TimeStamp();
            t.time = Time.realtimeSinceStartup;
       
            t.gameObject = g.gameObject;
            t.on = state;
            timeStamps.Add(t);
        }
    }

}
