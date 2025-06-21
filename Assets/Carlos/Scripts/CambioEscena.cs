using System;
using UnityEngine;

public class CambioEscena : MonoBehaviour, Interactuable
{
    [SerializeField] string nombreEscena;



    public void Interactuar()
    {
        GameManager.Instance.CargarEscenaPorNombre(nombreEscena);
    }
}
