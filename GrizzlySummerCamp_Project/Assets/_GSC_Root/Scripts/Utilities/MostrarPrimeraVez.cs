using UnityEngine;

public class MostrarPrimeraVez : MonoBehaviour
{
    [SerializeField] private GameObject contenedorObjeto; //objeto que se quiere ocultar despues de la primera vez en pantalla

    int storedData = 0;

    public string dataStoredName = "mensaje primera vez";

    [SerializeField] bool deleteStoredDataInEditor = false; //sirve para volver a ver la cinematica una vez ha pasado 

    private void Awake()
    {
        storedData = PlayerPrefs.GetInt(dataStoredName, 0);

        if (contenedorObjeto != null)
        {
            contenedorObjeto.SetActive(storedData == 0);
        }

        PlayerPrefs.SetInt(dataStoredName, 1);

    }

    private void OnValidate()
    {
        if (deleteStoredDataInEditor)
        {
            deleteStoredDataInEditor = false;
            PlayerPrefs.DeleteKey(dataStoredName);
        }
    }



}
