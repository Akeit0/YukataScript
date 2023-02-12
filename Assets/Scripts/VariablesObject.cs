using UnityEngine;
using YS.Fields;

namespace DefaultNamespace {
    [CreateAssetMenu(fileName = "Variables", menuName = "ScriptableObjects/VariablesObject")]
    public class VariablesObject :ScriptableObject {
        public NamedVariable[] Fields;
    }
}