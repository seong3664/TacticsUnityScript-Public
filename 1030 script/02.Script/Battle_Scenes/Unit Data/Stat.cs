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
        [SerializeField]
        private int _actionPoint;
        [SerializeField]
        private int _aiming;
        [SerializeField]
        private int _evasion;
        [SerializeField]
        private int _crit;
        public bool ScopeOnoff;
        public bool VestOnoff;
        public bool MuzzleOnoff;

        public UnitCode UnitCode {get {return unit;} set { unit = value; } }
        public int dmg { get { return Dmg; } set { Dmg = value; } }
        public int Hp { get { return _hp; } set { _hp = value;if(unit_Inspector != null) unit_Inspector.UpdateUInspector(); }  }
        public int MovePoint { get { return _movePoint; }  set { _movePoint = value; } }
        public int Action { get { return _actionPoint; } set { _actionPoint = value; if (unit_Inspector != null) unit_Inspector.UpdateUInspector(); } }
        public int Aiming { get { return _aiming; }  set { _aiming = value; } }
        public int Evasion { get { return _evasion; } set { _evasion = value; } }
        public int Crit { get { return _crit; }  set { _crit = value; } }

    }
}
