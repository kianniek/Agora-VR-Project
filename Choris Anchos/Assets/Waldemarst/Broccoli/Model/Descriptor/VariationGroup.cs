using System;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using Broccoli.Model;

namespace Broccoli.Pipe {
    /// <summary>
    /// Variation group class.
    /// </summary>
    [Serializable]
    public class VariationGroup {
        #region Vars
        /// <summary>
        /// Characters used to generate a random name for the framing.
        /// </summary>
        const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";
        public int id = 0;
        public string name = "";
        public bool enabled = true;
        public int seed = 0;
        public Vector2 frequency = new Vector2 (1, 4);
        public Vector3 center = Vector3.zero;
        public Vector2 radius = new Vector2 (0, 0);
        public float centerFactor = 0f;
        public enum OrientationMode {
            CenterToPeriphery,
            PeripheryToCenter,
            clockwise,
            counterClockwise
        }
        public OrientationMode orientation = OrientationMode.CenterToPeriphery;
        public float orientationRandomness = 0f;
        public Vector2 tiltAtCenter = new Vector2 (0, 0);
        public Vector2 tiltAtBorder = new Vector2 (0, 0);
        public Vector2 scaleAtCenter = new Vector2 (1, 1);
        public Vector2 scaleAtBorder = new Vector2 (1, 1);
        public enum BendMode {
            CenterToPeriphery,
            PeripheryToCenter,
            clockwise,
            counterClockwise
        }
        public BendMode bendMode = BendMode.CenterToPeriphery;
        public Vector2 bendAtCenter = new Vector2 (0, 0);
        public Vector2 bendAtBorder = new Vector2 (0, 0);
        public List<int> snapshotIds = new List<int> ();
        public enum GroupType {
            Steam,
            Branching
        }
        public GroupType groupType = GroupType.Steam;
        public bool isSteam {
            get { return groupType == GroupType.Steam;}
        }
        public Vector2 nodePosition = Vector2.zero;
        public int tag = -1;
        public int sidePanelOption = 0;
        #endregion

        #region Constructor
        public VariationGroup () {}
        #endregion

        #region Clone
        /// <summary>
        /// Clone this instance.
        /// </summary>
        public VariationGroup Clone () {
            VariationGroup clone = new VariationGroup ();
            clone.id = id;
            clone.name = name;
            clone.enabled = enabled;
            clone.seed = seed;
            clone.frequency = frequency;
            clone.center = center;
            clone.radius = radius;
            clone.centerFactor = centerFactor;
            clone.orientation = orientation;
            clone.orientationRandomness = orientationRandomness;
            clone.tiltAtCenter = tiltAtCenter;
            clone.tiltAtBorder = tiltAtBorder;
            clone.scaleAtCenter = scaleAtCenter;
            clone.scaleAtBorder = scaleAtBorder;
            clone.bendMode = bendMode;
            clone.bendAtCenter = bendAtCenter;
            clone.bendAtBorder = bendAtBorder;
            for (int i = 0; i < snapshotIds.Count; i++) {
                clone.snapshotIds.Add (snapshotIds [i]);
            }
            clone.groupType = groupType;
            clone.nodePosition = nodePosition;
            clone.tag = tag;
            clone.sidePanelOption = sidePanelOption;
            return clone;
        }
        /// <summary>
		/// Get a random string name.
		/// </summary>
		/// <param name="length">Number of characters.</param>
		/// <returns>Random string name.</returns>
        public static string GetRandomName (int length = 6) {
            string randomName = "";
            UnityEngine.Random.InitState ((int)System.DateTime.Now.Ticks);
            for(int i = 0; i < 6; i++) {
                randomName += glyphs [UnityEngine.Random.Range (0, glyphs.Length)];
            }
            return randomName;
        }
        #endregion

        #region Snapshot Management
        /// <summary>
        /// Adds a snapshot id to be part of this group.
        /// </summary>
        /// <param name="snapshotId">Id fo the snapshot.</param>
        /// <returns><c>True</c> if the snapshot gets added.</returns>
        public bool AddSnapshot (int snapshotId) {
            if (!snapshotIds.Contains (snapshotId)) {
                snapshotIds.Add (snapshotId);
            }
            return false;
        }
        /// <summary>
        /// Removes a snapshot from this group given its id.
        /// </summary>
        /// <param name="snapshotId">Id of the snapshot.</param>
        /// <returns><c>True</c> if the snapshot was removed.</returns>
        public bool RemoveSnapshot (int snapshotId) {
            int index = snapshotIds.IndexOf (snapshotId);
            if (index >= 0) {
                snapshotIds.RemoveAt (index);
                return true;
            }
            return false;
        }
        #endregion
    }
}