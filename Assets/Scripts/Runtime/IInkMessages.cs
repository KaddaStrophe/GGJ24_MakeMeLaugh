using UnityEngine;

namespace CatsInCostumes {
    interface IInkMessages {
        void OnSetInk(TextAsset ink);

        void OnAdvanceInk();

        void OnReact(string reaction);
    }
}
