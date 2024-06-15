var token = localStorage.getItem('token');
const urlParams = new URLSearchParams(window.location.search);
console.log(token);

let myId;
const to = urlParams.get('To');
console.log('To:', to);

const url = `http://localhost:5294/api/Messager/history?idToUser=${to}`;
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
        displayMessageSocket(data.Content, "message-other");
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
        displayMessageSocket(message, "message-my");
    } else {
        console.log('WebSocket is not connected');
    }
}

async function displayMessage(message, user) {
    const messageElement = document.createElement('div');
    messageElement.textContent = message;
    messageElement.classList.add(user);

    //messagesContainer.appendChild(messageElement);
    const container = document.querySelector('.history');
    container.insertBefore(messageElement, container.firstChild);
}

async function displayMessageSocket(message, user) {
    const messageElement = document.createElement('div');
    messageElement.textContent = message;
    messageElement.classList.add(user);

    messagesContainer.appendChild(messageElement);
    const container = document.querySelector('.web-socket');

    const chatfield = document.querySelector('.chatfield'); 
    chatfield.scrollTop = chatfield.scrollHeight;
}

async function getHistory(url, token) {
    const urlParams = new URLSearchParams(url);
    const count = urlParams.get('count');
    console.log('count:', count);

    const container = document.querySelector('.chatfield');

    const nowPosition = container.scrollHeight;
    console.log('СтараяВысота:', nowPosition);

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

            const newPosition = container.scrollHeight - nowPosition;

            if (count == null){
                const chatfield = document.querySelector('.chatfield');
                chatfield.scrollTop = chatfield.scrollHeight;
                connectWebSocket();
            } else {
                container.scrollTop = newPosition; 
                console.log('Позиция сейчас:', newPosition);
            }
        })
        .catch(error => {
            console.error('Ошибка', error);
        });
}

const chatfield = document.querySelector('.chatfield');
chatfield.addEventListener('scroll', function() {
    if (chatfield.scrollTop === 0) {
        console.log('Пользователь прокрутил содержимое .chatfield вверх');

        var history = document.querySelector('.history');
        var divHistory = history.getElementsByTagName('div');
        var divCountH = divHistory.length;

        var socket = document.querySelector('.web-socket');
        var divSocket = socket.getElementsByTagName('div');
        var divCountS = divSocket.length;

        console.log(divCountH+divCountS);

        getHistory(`http://localhost:5294/api/Messager/history?idToUser=${to}&count=${divCountH+divCountS+50}`, token);
    }
});