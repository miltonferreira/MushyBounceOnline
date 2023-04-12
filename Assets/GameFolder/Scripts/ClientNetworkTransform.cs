using Unity.Netcode.Components;
using UnityEngine;

namespace Unity.MultiPlayer.Samples.Utilities.ClientAuthority{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative(){
            return false;
        }
    }
}
