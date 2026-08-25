using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        // Cargar valor guardado
        slider.value = PlayerPrefs.GetFloat("Volumen", 0.7f);

        // Configurar evento
        slider.onValueChanged.AddListener(CambiarVolumen);
    }

    void CambiarVolumen(float valor)
    {
        // Cambiar volumen
        AudioListener.volume = valor;

        // Guardar
        PlayerPrefs.SetFloat("Volumen", valor);
    }
}
