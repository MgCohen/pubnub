const schema = require('@colyseus/schema');

class RoomConfig extends schema.Schema {
    constructor() {
        super();
        this.turnLength = 10000;
        this.playLength = 3000;
        this.seed = Math.random();
        this.maxPlayers = 4;
        this.teamCount = 2;
    }
}

schema.defineTypes(RoomConfig, {
    turnLength: "number",
    playLength: "number",
    seed: "number",
    maxPlayers: "number",
    teamCount: "number",
});

exports.RoomConfig = RoomConfig;

