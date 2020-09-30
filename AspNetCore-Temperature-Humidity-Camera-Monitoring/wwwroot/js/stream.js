(function() {
    const getWebSocketMessages = function (onMessageReceived) {
        const url = `ws://${location.host}/ws`;
        console.log('url is: ' + url);

        const webSocket = new WebSocket(url);

        webSocket.onmessage = onMessageReceived;
    };

    const ulElement = document.getElementById('stream');

    getWebSocketMessages(function (message) {
        ulElement.innerHTML = `<li>${message.data}</li>`
        console.log(message);
    });
}());