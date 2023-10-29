using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncaixeState : IPlayerState {
    Player player;
    Controls controls;

    Carga[] cargasAEncaixar;
    List<Carga> cargasEncaixadas;

    int currentSelected = -1;
    bool rodando = false; // Não mudar a variavel diretamente, usar SetRodando
    bool subiu = false;

    public Caixa caixaAtual {
        get {
            if (currentSelected < 0) return null;
            return cargasAEncaixar[currentSelected].cx;
        }
    }

    public System.Action<bool> onRotateChange;

    public EncaixeState(Player player, Carga[] cargasAEncaixar) {
        this.player = player;
        this.cargasAEncaixar = cargasAEncaixar;
    }

    public void Enter() {
        DefinirControles();
        UIController.encaixe.Mostrar();
        cargasEncaixadas = new List<Carga>(cargasAEncaixar.Length);

        for (int i = 0; i < cargasAEncaixar.Length; i++) {
            Carga carga = cargasAEncaixar[i];
            Transform ponto = player.pontosCaixa[i];
            InstanciarCaixa(carga, ponto);
        }

        // TODO: Ter um jeito de habilitar e desabilitar o player no próprio player
        player.GetComponent<Rigidbody>().isKinematic = true;

        // Seria interessante ter um próprio controlador de cameras ?
        player.cameras[0].gameObject.SetActive(false);
        player.cameras[1].gameObject.SetActive(true);

        player.cacambaTrigger.onTriggerEnter += OnTriggerEnter;
        player.cacambaTrigger.onTriggerExit += OnTriggerExit;

        Selecionar(0);
    }

    public void Execute(float dt) {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Mover(new Vector2(h,v), dt);
    }

    public void Exit() {
        // Lmebrando que o Exit pode ser chamado quando foi concluido e quando foi interrompido!
        UIController.encaixe.Esconder();
        
        player.gizmos.SetActive(false);
        controls.Encaixe.Disable();

        player.cameras[0].gameObject.SetActive(true);
        player.cameras[1].gameObject.SetActive(false);

        onRotateChange = null;

        // TODO: Ter um jeito de habilitar e desabilitar o player no próprio player
        Player.instance.GetComponent<Rigidbody>().isKinematic = false;
    }

    void DefinirControles() {
        controls = new Controls();
        controls.Encaixe.EfetuarAcao.performed += ctx => CheckConcluido();
        controls.Encaixe.Rotacionar.performed += ctx => SetRodando(!rodando);
        controls.Encaixe.Subir.performed += ctx => SubirCaixa(); // Imagino que a caixa sempre vai subir quando selecionar ela, não entendi direito o plano de separar isso... Talvez esse seja um trocar pro próximo?
        controls.Encaixe.Resetar.performed += ctx => caixaAtual?.ResetarPosicao();
        controls.Encaixe.Selecionar.performed += ctx => Selecionar(ctx.ReadValue<Vector2>());

        controls.Encaixe.Enable();
    }

    void InstanciarCaixa(Carga carga, Transform ponto) {
        GameObject caixaObj = GameObject.Instantiate(carga.prefab, ponto.position, carga.prefab.transform.rotation);
        Caixa caixa = caixaObj.GetComponent<Caixa>();

        carga.peso = caixa.GetComponent<Rigidbody>().mass;
        carga.fragilidade = caixa.GetComponent<Caixa>().carga.fragilidade;
        carga.fragilidadeInicial = carga.fragilidade;

        caixa.carga = carga;
        caixa.spawnPoint = ponto;

        carga.cx = caixa;
    }

    public void Selecionar(Vector2 dir) {
        int i = currentSelected;
        if (dir.x > 0) i++;
        else if (dir.x < 0) i--;
        
        if (dir.y > 0) i += 2;
        else if (dir.y < 0) i -= 2;

        Selecionar(i % cargasAEncaixar.Length);
    }

    public void Selecionar(int i) {
        i %= cargasAEncaixar.Length;
        caixaAtual?.Deselecionar();

        currentSelected = i;
        caixaAtual.Selecionar();
        subiu = false;
        SubirCaixa();

        player.gizmos.transform.position = caixaAtual.transform.position;
    }

    public void SubirCaixa() {
        if (caixaAtual == null) return;

        if (subiu) {
            caixaAtual.Deselecionar();
            return;
        }

        Transform pos = player.subirPos.transform;
        caixaAtual.gameObject.transform.position = new Vector3(pos.position.x, pos.position.y + 5f, pos.position.z);
        subiu = true;
    }

    public void Mover(Vector2 input, float dt = 1) {
        if (caixaAtual == null) return;

        if (rodando) {
            Vector3 rot = new Vector3(input.y, input.x, 0);
            rot = Player.instance.transform.TransformDirection(rot) * player.velocidadeRotacao;
            Quaternion delta = Quaternion.Euler(rot * dt);
            caixaAtual.MoveDeltaRotation(delta);
        } else {
            Vector3 mov = new Vector3(input.x, 0, input.y);
            mov = Player.instance.transform.TransformDirection(mov) * player.velocidade;
            caixaAtual.SetVelocity(mov * dt);
            player.gizmos.transform.position = caixaAtual.transform.position;
        }
    }
    
    public void SetRodando(bool rodando) {
        this.rodando = rodando;
        player.gizmos.SetActive(!rodando);

        onRotateChange?.Invoke(rodando);
    }

    public void ResetarPosicao() {
        if (caixaAtual == null) return;

        caixaAtual.ResetarPosicao();
        player.gizmos.transform.position = caixaAtual.transform.position;

        SetRodando(false);
    }

    void OnTriggerEnter(Collider other) {
        if(!other.gameObject.CompareTag("Entrega") || other.isTrigger) return;

        Carga carga = other.gameObject.GetComponent<Caixa>().carga;
        if (carga == null) return;
        if (cargasEncaixadas.Contains(carga)) return;

        cargasEncaixadas.Add(carga);
    }

    void OnTriggerExit(Collider other) {
        if(!other.gameObject.CompareTag("Entrega") || other.isTrigger) return;

        Carga carga = other.gameObject.GetComponent<Caixa>().carga;
        if (carga == null) return;
        if (!cargasEncaixadas.Contains(carga)) return;

        cargasEncaixadas.Remove(carga);
    }

    public void CheckConcluido() {
        if (cargasEncaixadas.Count == cargasAEncaixar.Length) {
            player.SetState(new DirigindoState(player));
            // cargasAtuais[r].gameObject.transform.SetParent(null); // acho que não precisa
        }
    }
}
