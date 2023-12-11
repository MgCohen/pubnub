const { Turn, PlayerTurn, PlayerAction } = require('./schema/Turn');

class TurnController {
    constructor(room) {
        this.state = room.state;
        this.clock = room.clock;
        this.config = room.config;
        this.room = room;
    }

    startTurns() {
        console.log("starting match");

        this.clock.start();
        this.turnTimer = this.clock.setInterval(() => this.playTurn(), this.config.turnLength);

        this.room.broadcast("matchStart");
    }

    playTurn() {
        console.log("playing turn");

        var turnCount = this.state.turns.push(this.state.currentTurn);
        this.room.broadcast("playTurn", this.state.currentTurn);
        this.state.currentTurn = new Turn().WithNumber(turnCount + 1);

        this.turnTimer.clear();
        this.turnTimer = this.clock.setInterval(() => this.playTurn(), this.config.turnLength + this.config.playLength);
        console.log("clock cleanup");
    }

    registerPlayerTurn(player, turn) {
        var turnNumber = turn.turnNumber;
        var playerTurn = turn.playerTurn;

        if (turnNumber != this.state.currentTurn.turnNumber) {
            console.log("trying to register move on the wrong turn");
            return;
        }

        this.state.currentTurn.RegisterPlayerTurn(player, playerTurn);
        this.previewTeamMove(player, turn);

        if (this.state.currentTurn.CheckTurns() == this.maxClients) {
            console.log("all moves for this turn where registered");
            this.playTurn();
        }
    }

    previewTeamMove(player, turn) {
        var team = this.state.teams[player.teamID];
        team.players.forEach(element => {
            element.client.send("previewMove", { playerID: player.playerID, playerTurn: turn });
        });
    }

}

exports.TurnController = TurnController;

