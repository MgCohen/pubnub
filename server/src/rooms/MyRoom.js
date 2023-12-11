const colyseus = require('colyseus');
const { MyRoomState } = require('./schema/MyRoomState');
const { Player } = require('./schema/Player');
const { Team } = require('./schema/Team');
const { Turn, PlayerTurn, PlayerAction} = require('./schema/Turn');
const { Delayed } = require('colyseus');
const {TurnController} = require('./TurnController');

exports.MyRoom = class extends colyseus.Room {

  onAuth(client, options, request) {
    this.checkPlayerState(client);
    return true;
  }

  checkPlayerState(client) {
    var id = client.id;
    if (!this.state.players.get(id)) {
      this.state.players.set(id, new Player().assign({
        playerID: id,
      }));
    }
    this.state.players.get(id).client = client;
  }

  onCreate(options) {
    this.maxClients = this.state.config.maxPlayers;

    this.setState(new MyRoomState());

    this.onMessage("RegisterMove", (client, data) => {
      var player = this.state.players[client.id];
      this.turnController.registerPlayerTurn(player, data);
    });
  }

  onJoin(client, options) {
    if (this.checkGameStart()) {
      this.splitTeams();
      this.turnController = new TurnController(this);
      this.turnController.startTurns();
    }
  }

  checkGameStart() {
    return this.state.players.size == this.maxClients;
  }

  splitTeams() {
    var currentTeam = 0;
    var playersPerTeam = this.state.config.maxPlayers / this.state.config.teamCount;
    var currentCount = 0;

    this.state.teams[currentTeam] = new Team();
    this.state.players.forEach((value, key) => {
      if (currentCount >= playersPerTeam) {
        currentTeam++;
        this.state.teams[currentTeam] = new Team();
        currentCount = 0;
      }
      currentCount++;
      this.state.teams[currentTeam].players.push(value);
      console.log((value.client != null && value.client != undefined));
      value.teamID = currentTeam;
    });
  }

  onLeave(client, consented) {
    console.log(client.sessionId, "left!");

    var player = this.state.players.get(client.id);
    if (player) {
      return;
    }
    // var teamID = player.teamID;
    this.state.players.delete(client.id);
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

}
