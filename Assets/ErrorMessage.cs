using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    public void Destroyhimself()
    {
        Destroy(this.gameObject);
    }
    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
