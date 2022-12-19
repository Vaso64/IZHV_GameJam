using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRotation : MonoBehaviour
{
    private void Update() => transform.Rotate(0, 5f * Time.deltaTime, 0);
}
