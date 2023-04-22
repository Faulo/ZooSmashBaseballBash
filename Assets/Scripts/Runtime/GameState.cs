using System.Collections.Generic;
using UnityEngine;

namespace ZSBB {
    [CreateAssetMenu]
    sealed class GameState : ScriptableObject {
        public IEnumerable<(string key, string value)> variables {
            get {
                yield return ("$version", Application.version);
                yield return ("$productName", Application.productName);
                yield return ("$companyName", Application.companyName);

                yield return ("$in", "0");
                yield return ("$out", "1");
                yield return ("$total", "1");

                yield return ("$stability", "🧱🧱🧱");
                yield return ("$timer", "0:59");
            }
        }
    }
}
