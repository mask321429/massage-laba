let socket;
const messagesContainer = document.getElementById('messages');

function connectWebSocket() {
    const userId = document.getElementById('userId').value;
    if (!userId) {
        alert("Please enter your User ID.");
        return;
    }

    const userIdf = document.getElementById('userIdf').value;
    if (!userIdf) {
        alert("Please enter your User ID.");
        return;
    }
    console.log(userIdf);
    console.log(userId);
    socket = new WebSocket(`ws://localhost:5294/ws?userId=${userId}`);

    socket.onopen = function() {
        displayMessage('WebSocket connection established');
    };

    socket.onerror = function(error) {
        displayMessage('WebSocket error: ' + JSON.stringify(error));
    };

    socket.onmessage = function(event) {
        const data = JSON.parse(event.data);
        displayMessage(`Message from ${data.FromUserId}: ${data.Content}`);
    };

    socket.onclose = function(event) {
        displayMessage(`WebSocket connection closed: ${event.code} - ${event.reason}`);
    };
}

function sendMessage() {
    const messageInput = document.getElementById('messageInput');
    const message = messageInput.value;
    const fromUserId = document.getElementById('userId').value;
    const userIdf = document.getElementById('userIdf').value;

    if (!message) {
        alert("Message cannot be empty.");
        return;
    }

    messageInput.value = '';

    if (socket && socket.readyState === WebSocket.OPEN) {
        socket.send(JSON.stringify({
            FromUserId: fromUserId,
            ToUserId: userIdf,
            Content: message
        }));
        displayMessage('Sent: ' + message);
    } else {
        displayMessage('WebSocket is not connected');
    }
}

function displayMessage(message) {
    const messageElement = document.createElement('div');
    messageElement.textContent = message;
    messagesContainer.appendChild(messageElement);
}