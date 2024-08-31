#region License and Information
/*****
* This class is a wrapper for Unity3d's Input class which allows to simulate
* an input delay. It provides similar methods as the Input class but with an
* additional delay parameter. Since the input processing is handled in Update
* it's recommended to set the DelayedInput script before the normal time in
* the script execution order.
* 
* This script requires this RingBuffer class:
*    https://www.dropbox.com/s/zvjnkvkeo2e2gtu/RingBuffer.cs?dl=0
* or https://pastebin.com/1twHCqXu
*  
* Copyright (c) 2018 Markus Göbel (Bunny83)
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to
* deal in the Software without restriction, including without limitation the
* rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
* sell copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
* IN THE SOFTWARE.
* 
*****/
#endregion License and Information

using System.Collections.Generic;
using UnityEngine;
using B83.Collections;

public class DelayedInput : MonoBehaviour
{
    #region singleton
    private static DelayedInput m_Instance = null;
    private static DelayedInput Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameObject("DelayedInput").AddComponent<DelayedInput>();
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }
    #endregion singleton
    private static int maxSamples = 200;
    public static int MaxSamples
    {
        get { return maxSamples; }
        set {
            maxSamples = value;
            foreach (var axis in Instance.m_Inputs)
                axis.history.Capacity = maxSamples;
        }
    }

    private List<InputAxis> m_Inputs = new List<InputAxis>();
    private void Update()
    {
        foreach (var axis in m_Inputs)
        {
            axis.Update();
        }
        m_ButtonQueueDown.Clear();
        m_ButtonQueueUp.Clear();
        m_KeyQueueDown.Clear();
        m_KeyQueueUp.Clear();
        float t = Time.unscaledTime;
        while (m_Events.Count > 0 && t > m_Events[m_Events.Count - 1].time)
        {
            var e = m_Events[m_Events.Count - 1];
            switch (e.type)
            {
                case EventType.ButtonDown:
                    m_ButtonQueueDown[e.buttonName] = e;
                    break;
                case EventType.ButtonUp:
                    m_ButtonQueueUp[e.buttonName] = e;
                    break;
                case EventType.KeyDown:
                    m_KeyQueueDown[e.key] = e;
                    break;
                case EventType.KeyUp:
                    m_KeyQueueUp[e.key] = e;
                    break;
            }
            m_Events.RemoveAt(m_Events.Count - 1);
        }
    }

    #region GetAxis
    private class InputAxis
    {
        public struct Value
        {
            public float value;
            public float time;
        }
        public string inputName;
        public KeyCode key;
        public RingBuffer<Value> history = new RingBuffer<Value>(maxSamples);
        public virtual void Update()
        {
            history.Add(new Value { value = Input.GetAxis(inputName), time = Time.unscaledTime });
        }

        public float Get(float aDelay)
        {
            if (history.Count == 0)
                Update();
            float d = Time.unscaledTime - aDelay;
            // binary search
            int min = 0;
            int max = history.Count-1;
            while (max - min > 1)
            {
                int mid = (min + max) / 2;
                float time = history[mid].time;
                if (time < d)
                    min = mid;
                else
                    max = mid;
            }
            return history[min].value;
        }
    }

    private Dictionary<string, InputAxis> m_InputAxes = new Dictionary<string, InputAxis>();
    private InputAxis GetAxisInternal(string aAxisName)
    {
        InputAxis obj;
        if (!m_InputAxes.TryGetValue(aAxisName, out obj))
        {
            obj = new InputAxis { inputName = aAxisName };
            m_InputAxes.Add(aAxisName, obj);
            m_Inputs.Add(obj);
        }
        return obj;
    }
    public static float GetAxis(string aAxis, float aDelay)
    {
        var axis = Instance.GetAxisInternal(aAxis);
        return axis.Get(aDelay);
    }
    #endregion GetAxis

    #region GetButton
    private class Button : InputAxis
    {
        public override void Update()
        {
            history.Add(new Value { value = Input.GetButton(inputName)?1:0, time = Time.unscaledTime });
        }
    }
    private Dictionary<string, Button> m_Buttons = new Dictionary<string, Button>();
    private Button GetButtonInternal(string aButtonName)
    {
        Button obj;
        if (!m_Buttons.TryGetValue(aButtonName, out obj))
        {
            obj = new Button { inputName = aButtonName };
            m_InputAxes.Add(aButtonName, obj);
            m_Inputs.Add(obj);
        }
        return obj;
    }
    public static bool GetButton(string aAxis, float aDelay)
    {
        var axis = Instance.GetButtonInternal(aAxis);
        return axis.Get(aDelay) > 0;
    }
    #endregion GetButton

    #region GetKey
    private class Key : InputAxis
    {
        public override void Update()
        {
            history.Add(new Value { value = Input.GetKey(key) ? 1 : 0, time = Time.unscaledTime });
        }
    }
    private Dictionary<KeyCode, Key> m_Keys = new Dictionary<KeyCode, Key>();
    private Key GetKeyInternal(KeyCode aKey)
    {
        Key obj;
        if (!m_Keys.TryGetValue(aKey, out obj))
        {
            obj = new Key { key = aKey};
            m_Keys.Add(aKey, obj);
            m_Inputs.Add(obj);
        }
        return obj;
    }
    public static bool GetKey(KeyCode aKey, float aDelay)
    {
        var axis = Instance.GetKeyInternal(aKey);
        return axis.Get(aDelay) > 0;
    }
    #endregion GetKey

    #region Events
    private enum EventType { KeyUp, KeyDown, ButtonUp, ButtonDown}
    private struct Event
    {
        public float time;
        public EventType type;
        public KeyCode key;
        public string buttonName;
    }

    private List<Event> m_Events = new List<Event>();
    private Dictionary<string, Event> m_ButtonQueueDown = new Dictionary<string, Event>();
    private Dictionary<string, Event> m_ButtonQueueUp = new Dictionary<string, Event>();
    private Dictionary<KeyCode, Event> m_KeyQueueDown = new Dictionary<KeyCode, Event>();
    private Dictionary<KeyCode, Event> m_KeyQueueUp = new Dictionary<KeyCode, Event>();

    private void AddEvent(Event aEvent)
    {
        m_Events.Add(aEvent);
        m_Events.Sort((a, b) => b.time.CompareTo(a.time));
    }

    public static bool GetButtonDown(string aButtonName, float aDelay)
    {
        if (Input.GetButtonDown(aButtonName))
        {
            Instance.AddEvent(new Event { type = EventType.ButtonDown, buttonName = aButtonName, time = Time.unscaledTime + aDelay });
        }
        Event res;
        if (Instance.m_ButtonQueueDown.TryGetValue(aButtonName, out res) && res.type == EventType.ButtonDown)
            return true;
        return false;
    }
    public static bool GetButtonUp(string aButtonName, float aDelay)
    {
        if (Input.GetButtonUp(aButtonName))
            Instance.AddEvent(new Event { type = EventType.ButtonUp, buttonName = aButtonName, time = Time.unscaledTime + aDelay });
        Event res;
        if (Instance.m_ButtonQueueUp.TryGetValue(aButtonName, out res) && res.type == EventType.ButtonUp)
            return true;
        return false;
    }
    public static bool GetKeyDown(KeyCode aKey, float aDelay)
    {
        if (Input.GetKeyDown(aKey))
            Instance.AddEvent(new Event { type = EventType.KeyDown, key = aKey, time = Time.unscaledTime + aDelay });
        Event res;
        if (Instance.m_KeyQueueDown.TryGetValue(aKey, out res) && res.type == EventType.KeyDown)
            return true;
        return false;
    }
    public static bool GetKeyUp(KeyCode aKey, float aDelay)
    {
        if (Input.GetKeyUp(aKey))
            Instance.AddEvent(new Event { type = EventType.KeyUp, key = aKey, time = Time.unscaledTime + aDelay });
        Event res;
        if (Instance.m_KeyQueueUp.TryGetValue(aKey, out res) && res.type == EventType.KeyUp)
            return true;
        return false;
    }

    #endregion Events
}
