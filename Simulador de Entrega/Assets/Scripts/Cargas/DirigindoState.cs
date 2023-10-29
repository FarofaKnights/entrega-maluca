using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirigindoState : IPlayerState {
    // State meio bucha mas é pq o código do iago tem muita variável então não quero mexer nele, não agora pelo menos
    Player player;
    WhellControler whellControler;

    public DirigindoState(Player player) {
        this.player = player;
        whellControler = player.GetComponent<WhellControler>();
    }

    public void Enter() {
        player.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Execute(float dt) {
        whellControler.Execute(dt);
    }

    public void Exit() { }
}
