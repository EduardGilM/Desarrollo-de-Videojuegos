using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimientocamara : MonoBehaviour
{
    public Transform rotCamara;
    public KeyCode tecla;
    public float duracion = 1f;

    private bool estaRotando = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(tecla) && !estaRotando)
        {
            StartCoroutine(RotarCamara());
        }
    }

    IEnumerator RotarCamara()
    {
        estaRotando = true;
        float tiempo = 0;
        Quaternion inicio = rotCamara.rotation;
        Quaternion fin = inicio * Quaternion.Euler(0, -90, 0); // Rotar 45 grados hacia la derecha

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, tiempo / duracion); // Ease-in ease-out
            rotCamara.rotation = Quaternion.Slerp(inicio, fin, t);
            yield return null;
        }

        rotCamara.rotation = fin;
        estaRotando = false;
    }
}
