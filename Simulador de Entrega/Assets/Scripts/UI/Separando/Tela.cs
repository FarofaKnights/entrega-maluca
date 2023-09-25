public class Tela : MonoBehaviour {
    public bool exclusivo = false;
    public string nome;

    Tela[] GetVizinhas() {
        return transform.parent.GetComponentsInChildren<Tela>();
    }

    public void Mostrar() {
        if (exclusivo) {
            foreach (Tela t in GetVizinhas()) {
                if (t != this && t.exclusivo)
                    t.Esconder();
            }
        }

        gameObject.SetActive(true);
    }

    public void Esconder() {
        gameObject.SetActive(false);
    }

    public bool visivel {
        get { return gameObject.activeSelf; }
    }
}
