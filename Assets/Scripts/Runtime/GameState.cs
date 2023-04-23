using System.Collections.Generic;
using UnityEngine;

namespace ZSBB {
    [CreateAssetMenu]
    sealed class GameState : ScriptableObject {
        [SerializeField]
        AnimalManager animals;

        public IEnumerable<(string key, string value)> variables {
            get {
                yield return ("$version", Application.version);
                yield return ("$productName", Application.productName);
                yield return ("$companyName", Application.companyName);

                yield return ("$in", animals.inCount.ToString());
                yield return ("$out", animals.outCount.ToString());
                yield return ("$total", animals.totalCount.ToString());

                yield return ("$stability", "x"); // 🧱
                yield return ("$timer", "0:59");
            }
        }
    }
}
