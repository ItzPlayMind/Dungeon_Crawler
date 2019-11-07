var io = require("socket.io")(process.env.PORT || 4567);
var Player = require("./Player.js");
var Vector3 = require("./Vector3.js");

console.log('Server started!');

var players = [];
var sockets = [];

var connectedAmount = 0;

var redTeam = false;

io.on("connection", (socket) => {
    console.log("New Client connected!");

    connectedAmount++;
    var player = new Player();
    var thisPlayerID = player.id;

    players[thisPlayerID] = player;
    sockets[thisPlayerID] = socket;

    player.isRedTeam = redTeam;
    redTeam = !redTeam;
    
    socket.emit("setup", { id: thisPlayerID, isRedTeam: player.isRedTeam });//Tell myself to Spawn!
    socket.broadcast.emit("new player setup", player);//Tell everyone I spawned!

    //Tell myself who is already in game!
    for (var playerID in players) {
        if (playerID != thisPlayerID) {
            socket.emit("new player setup", players[playerID]);
        }
    }

    socket.on("update position", (data) => {
        player.position = new Vector3(data.position.x, data.position.y, data.position.z);
        player.rotation = new Vector3(data.rotation.x, data.rotation.y, data.rotation.z);
        socket.broadcast.emit("update position", player);
    });

    socket.on("damage", (data) => {
        var sendData = {
            id: data.id,
            damage: data.damage,
            attackerID: thisPlayerID
        }
        socket.broadcast.emit("damage", sendData);
    });

    socket.on("use ability", (data) => {
        socket.broadcast.emit("use ability", data);
    });

    socket.on("buy item", (data) => {
        var x = [];
        for (var i = 1; i <= 6; i++) {
            x.push(data["item" + i]);
        }
        console.log(x);
        player.items = x;
        socket.broadcast.emit("buy item", data);
    });

    socket.on("disconnect", (socket) => {
        redTeam = players[thisPlayerID].isRedTeam;
        console.log("Client disconnected!");
        connectedAmount--;
        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
        io.emit("remove player", { id: thisPlayerID });
    });
});
