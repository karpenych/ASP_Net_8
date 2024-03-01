'use strict';

var connection = null;
window.onload = function ()
{
    connection = new signalR.HubConnectionBuilder().withUrl("/Chat").build();
    connection.serverTimeoutInMilliseconds = 5 * 60 * 1000;

    connection.on('Notify', function (message) {
        document.getElementById('ntf').innerHTML += message + '<br>';
    });

    connection.on("UpdateData", function (user, message, str_time, prc) {
        document.getElementById('tb').innerHTML +=
        '<tr><td>' + user + '</td><td>' + message + '</td>' +
        '<td>' + str_time + '</td><td>' + prc + '</td></tr>';
    });

    connection.start().then(function () {
        connection.invoke('OnConnectedAsync', null)
            .catch(function (err) { return console.error(err.toString()); });
    });
}

let prev_user = '';
let prev_message = 0;
let prc = 0;
let user = '';
let message = '';

document.getElementById('sendButton').addEventListener('click', (event) =>
    f_SendButton(event));

function f_SendButton(event) {
    user = document.getElementById('userInput').value;
    message = document.getElementById('messageInput').value;

    if (prev_user === '') {
        prev_user = user;
        document.getElementById('userInput').disabled = true;
        prev_message = message;
    }

    prc = (message - prev_message) / prev_message * 100;
    prc = '' + prc.toFixed(2);
    document.getElementById('prev_messageInput').value = prev_message;
    prev_message = message;

    let time = new Date();
    let str_time = time.getHours() + ':' + time.getMinutes() + ':' + time.getMilliseconds();

    connection.invoke('SendMessage', '' + user + '@' + message + '@' + str_time + '@' +
        prc).catch(function (err) {
            return console.error(err.toString());
        });

    event.preventDefault();
}

window.onended = function () {
    connection.invoke('OnDisconnectedAsync', null).catch(function (err) {
        return console.error(err.toString());
    });
}
