using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sebanne.GripFit.Editor
{
    [Serializable]
    internal sealed class GripFitPendingRecord
    {
        public int targetInstanceId;
        public Vector3 position;
        public Quaternion rotation;
    }

    [Serializable]
    internal sealed class GripFitPendingList
    {
        public List<GripFitPendingRecord> records = new();
    }
}
