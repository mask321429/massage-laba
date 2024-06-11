var token = localStorage.getItem('token');
const urlParams = new URLSearchParams(window.location.search);

const from = urlParams.get('From');
const to = urlParams.get('To');
const name = urlParams.get('Name');

console.log('From:', from);
console.log('To:', to);
console.log('Name:', name);

document.getElementById('chatName').textContent = name;

connectWebSocket();
const messagesContainer = document.getElementById('web-socket');

function connectWebSocket() {

    socket = new WebSocket(`ws://localhost:5294/ws?userId=${from}`);

    socket.onopen = function() {
        console.log('WebSocket connection established');
    };

    socket.onerror = function(error) {
        console.log('WebSocket error: ' + JSON.stringify(error));
    };

    socket.onmessage = function(event) {
        const data = JSON.parse(event.data);
        displayMessage(data.Content, "message-other");
    };

    socket.onclose = function(event) {
        console.log(`WebSocket connection closed: ${event.code} - ${event.reason}`);
    };
}


function sendMessage() {
    const messageInput = document.getElementById('messageInput');
    const message = messageInput.value;

    messageInput.value = '';

    if (socket && socket.readyState === WebSocket.OPEN) {
        socket.send(JSON.stringify({
            FromUserId: from,
            ToUserId: to,
            Content: message
        }));
        displayMessage(message, "message-my");
    } else {
        console.log('WebSocket is not connected');
    }
}

function displayMessage(message, user) {
    const messageElement = document.createElement('div');
    messageElement.textContent = message;
    messageElement.classList.add(user);
    messagesContainer.appendChild(messageElement);
    const container = document.querySelector('.chatfield');
    container.scrollTop = container.scrollHeight;
}