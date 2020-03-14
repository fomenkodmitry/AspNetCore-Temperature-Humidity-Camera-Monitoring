(function() {
    var getWebSocketMessages = function(onMessageReceived)
    {
        var url = `ws://${location.host}/ws`
        console.log('url is: ' + url);

        var webSocket = new WebSocket(url);

        webSocket.onmessage = onMessageReceived;
    };

    var ulElement = document.getElementById('stream');

    getWebSocketMessages(function (message) {
        ulElement.innerHTML = `<li>${message.data}</li>`
        console.log(message);
    });
}());