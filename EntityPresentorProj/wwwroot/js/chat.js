"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {

var img =document.getElementById("paintPic");
img.src= message;
document.getElementById("dateUpdate").value=user;

});

connection.start().then(function () {


}).catch(function (err) {
    return console.error(err.toString());
});
