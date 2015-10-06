using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class DelayedInvokeMarshal : MonoBehaviour
    {
        private class InvocationEntry
        {
            public Action Action { get; set; }
            public float? TimeToRun { get; set; }
            public float TimeDelay { get; set; }
            public bool IsRealtime { get; set; }
        }

        private class GuidEqualityComparer : IEqualityComparer<Guid>
        {
            public bool Equals(Guid x, Guid y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(Guid obj)
            {
                return obj.GetHashCode();
            }
        }

        private Dictionary<Guid, InvocationEntry> _invocationQueue = new Dictionary<Guid, InvocationEntry>(new GuidEqualityComparer());

        private static DelayedInvokeMarshal _instance;

        public static DelayedInvokeMarshal Instance
        {
            get { return _instance; }
        }

        private static float ingameTimeSinceStartup = 0.0f;

        /// <summary>
        /// Returns a handle to the enqueued entry. You may use this handle to cancel the entry before it's invoked.
        /// </summary>
        public Guid Enqueue(Action action, float realtimeDelay, bool isRealtime = true)
        {
            lock (_invocationQueue)
            {
                var handle = Guid.NewGuid();
                _invocationQueue.Add(handle, new InvocationEntry()
                {
                    Action = action,
                    TimeDelay = realtimeDelay,
                    IsRealtime = isRealtime,
                });

                return handle;
            }
        }

        public void Cancel(Guid handle)
        {
            lock (_invocationQueue)
            {
                if (!_invocationQueue.ContainsKey(handle)) return;
                _invocationQueue.Remove(handle);
            }
        }

        public void CancelAll()
        {
            lock (_invocationQueue)
            {
               _invocationQueue.Clear();
            }
        }

        public void Awake()
        {
            _instance = this;
        }

        public void Update() {
            ingameTimeSinceStartup += Time.deltaTime;
            lock (_invocationQueue)
            {
                var firstSeenEntries = _invocationQueue.Where(e => e.Value.TimeToRun == null).ToList();
                foreach (var invocationEntry in firstSeenEntries)
                {
                    invocationEntry.Value.TimeToRun = invocationEntry.Value.IsRealtime ? Time.realtimeSinceStartup + invocationEntry.Value.TimeDelay : ingameTimeSinceStartup + invocationEntry.Value.TimeDelay;
                }

                var triggeredEntries = _invocationQueue.Where(e => ((e.Value.TimeToRun < Time.realtimeSinceStartup) && e.Value.IsRealtime) || ((e.Value.TimeToRun < ingameTimeSinceStartup) && e.Value.IsRealtime == false)).ToList();
                foreach (var invocationEntry in triggeredEntries)
                {
                    invocationEntry.Value.Action();
                    _invocationQueue.Remove(invocationEntry.Key);
                }
            }
        }
    }
}
