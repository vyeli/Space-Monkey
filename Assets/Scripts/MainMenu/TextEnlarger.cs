using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnlarger : MonoBehaviour
{
    public float scaleFactor = 1.2f; // Factor de escala
    public float duration = 1.0f;    // Duración de la animación

    private float timer;
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Calcula una escala que varía entre el valor original y el valor original multiplicado por el factor de escala
        float scale = Mathf.Lerp(initialScale.x, initialScale.x * scaleFactor, Mathf.PingPong(timer / duration, 1f));

        // Aplica la escala al objeto de texto
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
