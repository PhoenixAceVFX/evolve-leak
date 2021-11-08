namespace Evolve.Modules
{

    using UnityEngine;

    internal interface IControlScheme
    {
        bool HandleInput(Transform playerTransform, Transform cameraTransform);
    }

}