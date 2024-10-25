using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public enum UnitCode {Player,Enemy}
    [CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Unit Data", order = int.MaxValue)]
    public class Stat : ScriptableObject
    {
        public Unit_inspector_Ctrl unit_Inspector;

        [SerializeField]
        private int Dmg;
        [SerializeField]
        private UnitCode unit;
        [SerializeField]
        private int _hp;
        [SerializeField]
        private int _movePoint;
        private int _actionPoint;
        [SerializeField]
        private int _aiming;
        [SerializeField]
        private int _evasion;
        
        public UnitCode UnitCode {get {return unit;}}
        public int dmg { get { return Dmg; } private set { Dmg = value; } }
        public int Hp { get { return _hp; } set { _hp = value; unit_Inspector.UpdateUInspector(); }  }
        public int MovePoint { get { return _movePoint; }  private set { _movePoint = value; } }
        public int Action { get { return _actionPoint; } set { _actionPoint = value; unit_Inspector.UpdateUInspector(); } }
        public int Aiming { get { return _aiming; } protected set { _aiming = value; } }
        public int Evasion { get { return _evasion; } set { _evasion = value; } }

       
    }
}
