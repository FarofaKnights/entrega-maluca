using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaCena : MonoBehaviour
{
    public string rota;

    public void Troca()
    {
        SceneManager.LoadScene(rota);
    }
}
