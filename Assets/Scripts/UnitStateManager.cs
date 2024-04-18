using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public enum UnitState { Idle, IsMoving, Attacking}
    public class UnitStateManager : MonoBehaviour {

        public bool IsIdle(UnitState state) { return state == UnitState.Idle; }
        public bool IsMoving(UnitState state) { return state == UnitState.IsMoving; }
        public bool IsAttacking(UnitState state) { return state == UnitState.Attacking; }
    }
}