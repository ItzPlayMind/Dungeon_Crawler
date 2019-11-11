var shortID = require("shortid");
var Vector3 = require("./Vector3.js");

module.exports = class Player{
    constructor(){
        this.id = shortID.generate();
        this.position = new Vector3(0, 0, 0);
        this.rotation = new Vector3(0, 0, 0);
        this.level = 1;
        this.xp = 0;
        this.items = [];
        for (var i = 0; i < 6; i++) {
            this.items.push("");
        }
        this.skills = [];
        for (var i = 0; i < 4; i++) {
            this.skills.push("");
        }
        this.isRedTeam = false;
    }
}