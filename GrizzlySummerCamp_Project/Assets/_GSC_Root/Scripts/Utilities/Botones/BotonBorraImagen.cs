using UnityEngine;

public class BotonBorraImagen : MonoBehaviour
{
    public GameObject imageToDestroy;
    public string idImage = "image1Destroyed";

    void Start()
    {
        if (PlayerPrefs.GetInt(idImage, 0) == 1)
            imageToDestroy.SetActive(false);
    }

    public void DestroyImage()
    {
        imageToDestroy.SetActive(false);

        PlayerPrefs.SetInt(idImage, 1);
        PlayerPrefs.Save();
    }
}
