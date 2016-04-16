using UnityEngine;
using System.Collections;

public interface IEnemyBehaviour {

    void DoUpdate();

    void InitBehaviour(Enemy enemy);
}
