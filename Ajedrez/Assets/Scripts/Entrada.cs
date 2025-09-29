using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrada : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] public KeyCode teclaDisparo = KeyCode.Space;

    [Header("Movimiento")]
    [SerializeField] public Transform objetoAMover;
    [SerializeField] private float alturaDestino;
    [SerializeField, Min(0.01f)] private float duracionMovimiento = 1f;
    [SerializeField] private AnimationCurve interpolacion = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Coroutine movimientoActivo;

    private void Reset()
    {
        objetoAMover = transform;
        alturaDestino = transform.position.y;
    }

    private void Awake()
    {
        if (objetoAMover == null)
        {
            objetoAMover = transform;
            alturaDestino = objetoAMover.position.y;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(teclaDisparo))
        {
            IntentarIniciarMovimiento();
        }
    }

    private void IntentarIniciarMovimiento()
    {
        if (objetoAMover == null)
        {
            Debug.LogWarning("Entrada: falta referencia del objeto a mover.", this);
            return;
        }

        if (movimientoActivo != null)
        {
            StopCoroutine(movimientoActivo);
        }

        movimientoActivo = StartCoroutine(MoverHastaDestino());
    }

    private IEnumerator MoverHastaDestino()
    {
        Vector3 posicionInicial = objetoAMover.position;
        Vector3 posicionObjetivo = new Vector3(posicionInicial.x, alturaDestino, posicionInicial.z);

        if (Mathf.Abs(posicionInicial.y - alturaDestino) <= 0.001f)
        {
            objetoAMover.position = posicionObjetivo;
            movimientoActivo = null;
            yield break;
        }

        float tiempo = 0f;

        while (tiempo < duracionMovimiento)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracionMovimiento);
            float factor = interpolacion != null ? interpolacion.Evaluate(t) : t;
            objetoAMover.position = Vector3.Lerp(posicionInicial, posicionObjetivo, factor);
            yield return null;
        }

        objetoAMover.position = posicionObjetivo;
        movimientoActivo = null;
    }
}
