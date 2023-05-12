/*
public class Iniciavel {
    public Iniciavel pai;
    public Iniciavel[] filhos;
    protected int indiceFilhos = 0;

    public Diretriz diretriz;
    public bool sequencial = true;

    public Iniciavel() {
        this.pai = null;
        this.filhos = null;
        this.diretriz = null;
        this.indiceFilhos = 0;
    }

    public Iniciavel(Iniciavel[] filhos) {
        this.pai = null;
        this.filhos = filhos;
        this.diretriz = null;
    }

    public void Iniciar() {
        indiceFilhos = 0;

        if (filhos != null) {
            if (sequencial) filhos[indiceFilhos].Iniciar();
            else foreach (Iniciavel filho in filhos) filho.Iniciar();
        }

        if (diretriz != null) diretriz.Iniciar();
    }

    public void Interromper() {
        if (filhos != null) {
            if (sequencial) filhos[indiceFilhos].Interromper();
            else foreach (Iniciavel filho in filhos) filho.Interromper();
        }

        if (diretriz != null) diretriz.Interromper();
    }

    public void Finalizar() {
        indiceFilhos = 0;

        if (diretriz != null) diretriz.Finalizar();

        if (pai != null) pai.ProximoFilho();
    }

    public void ProximoFilho() {
        if (filhos == null) return;

        indiceFilhos++;

        if(indiceFilhos >= filhos.Length - 1) Finalizar();
        else if (sequencial) filhos[indiceFilhos].Iniciar();
    }
}
*/