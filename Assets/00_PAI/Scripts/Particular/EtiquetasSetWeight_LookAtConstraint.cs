using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;

public class EtiquetasSetWeight_LookAtConstraint : MonoBehaviour
{
    public LookAtConstraint lookAtConstraint;

    public void SetWeightByTransform(Transform target, float newWeight)
    {
        if (lookAtConstraint == null) return;

        List<ConstraintSource> sources = new List<ConstraintSource>();
        lookAtConstraint.GetSources(sources);

        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].sourceTransform == target)
            {
                ConstraintSource source = sources[i];
                source.weight = newWeight;
                sources[i] = source;
                break;
            }
        }

        lookAtConstraint.SetSources(sources);
    }
}