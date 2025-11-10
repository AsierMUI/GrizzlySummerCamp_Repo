using NUnit.Framework.Internal.Filters;
using System.Collections;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] float duracion = 5f;

    void Start()
    {
        StartCoroutine(DesactivarTrasTiempo());
    }
    IEnumerator DesactivarTrasTiempo()
    {
        yield return new WaitForSeconds(duracion);
        gameObject.SetActive(false);
    }
}
