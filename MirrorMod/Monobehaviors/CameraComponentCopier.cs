using Nautilus.Extensions;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace MirrorMod.Monobehaviors
{
    internal class CameraComponentCopier : MonoBehaviour
    {
        private void Start()
        {
            Camera mainCamera = Camera.main;

            gameObject.SetActive(false);

            PostProcessingBehaviour behaviour = gameObject.AddComponent<PostProcessingBehaviour>().CopyComponent(mainCamera.GetComponent<PostProcessingBehaviour>());
            behaviour.profile.screenSpaceReflection.enabled = false;

            ColorCorrection cc = gameObject.AddComponent<ColorCorrection>().CopyComponent(mainCamera.GetComponent<ColorCorrection>());
            cc.behaviour = behaviour;
            gameObject.AddComponent<LensWater>().CopyComponent(mainCamera.GetComponent<LensWater>());
            gameObject.AddComponent<LensWaterController>().CopyComponent(mainCamera.GetComponent<LensWaterController>());
            gameObject.AddComponent<WaterscapeVolumeOnCamera>().CopyComponent(mainCamera.GetComponent<WaterscapeVolumeOnCamera>());

            ShowOnThisCamera showOnCamera = gameObject.GetComponent<ShowOnThisCamera>();
            showOnCamera.preCullActions.Add(() => Player.main.SetHeadVisible(true));
            showOnCamera.postCullActions.Add(() => Player.main.SetHeadVisible(false));

            //Causes memory leak ->
            //gameObject.EnsureComponent<WaterSurfaceOnCamera>().CopyComponent(mainCamera.GetComponent<WaterSurfaceOnCamera>());
            behaviour.enabled = false;
            gameObject.SetActive(true);
        }
    }
}
