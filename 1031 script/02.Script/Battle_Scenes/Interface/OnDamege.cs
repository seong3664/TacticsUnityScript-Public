using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OnDamege
{
    void OnDamege(int Damege,Vector3 hitPoint,Vector3 hitnom);
    void OnMissed();
  
}
