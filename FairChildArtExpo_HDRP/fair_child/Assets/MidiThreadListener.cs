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

    public List<TimeStamp> rollingStamps;
    public bool generateNewTimeStamps = false;
    public List<TimeStamp> timeStamps = new List<TimeStamp>();

    [System.Serializable]
    public class TimeStamp
    {
        public float time;
        public GameObject gameObject;
        public bool on;
        public Vector3 destination;
        public MidiRoller mr;
    }

    public class timeStampDestination
    {
        public TimeStamp ts;
        public Vector3 rollingDestination;
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
                t.mr = t.gameObject.GetComponent<MidiRoller>();
                index++;
            }
        }
    }

    public IEnumerator PlayTimeStamp(TimeStamp t)
    {
        bool complete = false;
        yield return new WaitForSeconds(t.time+0.1f);
        
        if (t.on)
        {
            if(!rollingStamps.Contains(t))
                rollingStamps.Add(t);
            //StartCoroutine(RotateForward(t.gameObject.transform, t.gameObject.transform.rotation, Quaternion.Euler(eulers), 0.1f, 0.1f));
            t.mr.destination = Quaternion.Euler(eulers);
            complete = true;
        }
        else
        {
            if (!rollingStamps.Contains(t))
                rollingStamps.Add(t);
            //StartCoroutine(RotateForward(t.gameObject.transform, t.gameObject.transform.rotation, Quaternion.Euler(-eulers), 0.2f, 0.1f));
            t.mr.destination = Quaternion.Euler(Vector3.zero);
            complete = true;
        }
    }

    public void FixedUpdate()
    {
       
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
        foreach (TimeStamp t in rollingStamps)
        {
            if (Vector3.Distance(t.gameObject.transform.rotation.eulerAngles, t.destination) <= 0.1f)
            {
                t.gameObject.transform.rotation = Quaternion.Euler(t.destination);
            }
        }
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
