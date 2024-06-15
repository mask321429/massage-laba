var token = localStorage.getItem('token');
const urlParams = new URLSearchParams(window.location.search);
console.log(token);

let myId;
const to = urlParams.get('To');
console.log('To:', to);

const url = `http://localhost:5294/api/Messager/history?idToUser=${to}&count=50`;
getHistory(url, token);

const messagesContainer = document.getElementById('web-socket');

async function connectWebSocket() {

    socket = new WebSocket(`ws://localhost:5294/ws?userId=${myId}`);
    console.log(myId);

    socket.onopen = function () {
        console.log('WebSocket connection established');
    };

    socket.onerror = function (error) {
        console.log('WebSocket error: ' + JSON.stringify(error));
    };

    socket.onmessage = function (event) {
        const data = JSON.parse(event.data);
        displayMessage(data.Content, "message-other");
    };

    socket.onclose = function (event) {
        console.log(`WebSocket connection closed: ${event.code} - ${event.reason}`);
    };
}


async function sendMessage() {
    const messageInput = document.getElementById('messageInput');
    const message = messageInput.value;

    messageInput.value = '';

    if (socket && socket.readyState === WebSocket.OPEN) {
        socket.send(JSON.stringify({
            FromUserId: myId,
            ToUserId: to,
            Content: message
        }));
        displayMessage(message, "message-my");
    } else {
        console.log('WebSocket is not connected');
    }
}

async function displayMessage(message, user) {
    const messageElement = document.createElement('div');
    messageElement.textContent = message;
    messageElement.classList.add(user);
    messagesContainer.appendChild(messageElement);
    const container = document.querySelector('.chatfield');
    container.scrollTop = container.scrollHeight;
}

async function getHistory(url, token) {
    return fetch(url, {
        method: 'GET',
        headers: new Headers({
            "Authorization": `Bearer ${token}`
        }),
    })
        .then(response => response.json())
        .then(data => {
            console.log(url);
            console.log(data);
            document.getElementById('chatName').textContent = data[0].name;
            myId = data[0].myId;
            console.log(myId);
            console.log(data[0].name);

            data[0].messages.forEach(function (element) {
                if (element.whoseMessage == myId) {
                    displayMessage(element.text, "message-my");
                } else {
                    displayMessage(element.text, "message-other");
                }
            });
            connectWebSocket();

        })
        .catch(error => {
            console.error('Ошибка', error);
        });
}

const chatfield = document.querySelector('.chatfield');
chatfield.addEventListener('scroll', function() {
    if (chatfield.scrollTop === 0) {
        console.log('Пользователь прокрутил содержимое .chatfield вверх');
    }
});