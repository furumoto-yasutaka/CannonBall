using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ITutorialHitBlock
{

    public IReadOnlyReactiveProperty<bool> GetIsSuccess();
}
