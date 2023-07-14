using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Symbol")]
public class Symbol : ScriptableObject {
        public string id;
        public Sprite sprite;
        public int[] payouts;
}
