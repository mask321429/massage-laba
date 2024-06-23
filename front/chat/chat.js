var token = localStorage.getItem('token');
const urlParams = new URLSearchParams(window.location.search);
console.log(token);
let messageCount = 0;
let myId;
let type = 0;
var filePath;
const to = urlParams.get('To');
console.log('To:', to);

const url = `http://localhost:5294/api/Messager/history?idToUser=${to}`;
getHistory(url, token);

const messagesContainer = document.getElementById('web-socket');
const messageInput = document.getElementById('messageInput');

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
        console.log(event.data);
        console.log(messageCount);
        messageCount++; // Increment the message count for each message received
        if (messageCount % 2 == 0) { // Check if the message count is even (skipping every second message)
            if (data.TypeMessage == 1) {
                //receiveImageChunk(data.Content); - Разделение фотографии
            }
            displayMessageSocket(data.Content, "message-other", data.TypeMessage);
        }
    };

    socket.onclose = function (event) {
        console.log(`WebSocket connection closed: ${event.code} - ${event.reason}`);
    };
}


async function sendMessage() {
    const message = messageInput.value;
    messageInput.value = '';
    if (socket && socket.readyState === WebSocket.OPEN) {
        // if (type === 1) {
        //     const imageInput = document.getElementById("fileInput");
        //     const file = imageInput.files[0];
        //     const chunkSize = 1024 * 32; // Define the chunk size
        //     const reader = new FileReader();
            
        //     reader.onload = function(event) {
        //         const imageData = event.target.result;
                
        //         if (typeof imageData === 'string') {
        //             const totalSize = imageData.length;
        //             let offset = 0;
        //             console.log(totalSize);
        //             while (offset < totalSize) {
        //                 const chunk = imageData.slice(offset, offset + chunkSize);
        //                 socket.send(JSON.stringify({
        //                     FromUserId: myId,
        //                     ToUserId: to,
        //                     TypeMessage: type,
        //                     Content: chunk
        //                 }));
        //                 offset += chunkSize;
        //             }
        //             console.log(imageData);
        //             console.log("Image data sent in chunks");
        //         } else {
        //             console.log("Error: Image data is not in a valid format");
        //         }
        //     };
            
        //     reader.readAsDataURL(file);
        // } else {
            socket.send(JSON.stringify({
                FromUserId: myId,
                ToUserId: to,
                TypeMessage: type,
                Content: message
            }));
            displayMessageSocket(message, "message-my", type);
        
    } else {
        console.log('WebSocket is not connected');
    }
}

async function displayMessage(message, user, type) {
    const messageElement = document.createElement('div');
    messageElement.textContent = message;
    messageElement.classList.add(user);

    if (type == 2) {
        var longitude = parseFloat(message.match(/Долгота: (.*?),/)[1]);
        var latitude = parseFloat(message.match(/Широта: (.*?)$/)[1]);
        messageElement.innerHTML = `<iframe src="https://www.openstreetmap.org/export/embed.html?bbox=${longitude}%2C${latitude}&layer=mapnik&marker=${latitude},${longitude}" width="100%" height="300" frameborder="0" scrolling="no"></iframe>`;
    }
    if (type == 1) {
        //image = btoa(message);
        //const newPath = message.replace('C:\\fakepath\\', '../../Фоточки/');
        messageElement.innerHTML = `<img src="../../Фоточки/${message}" width="100%" height="300" frameborder="0" scrolling="no"></img>`;
    }

    const container = document.querySelector('.history');
    container.insertBefore(messageElement, container.firstChild);
}

async function displayMessageSocket(message, user, type) {
    const messageElement = document.createElement('div');

    messageElement.textContent = message;
    messageElement.classList.add(user);

    messagesContainer.appendChild(messageElement);

    if (type == 2) {
        var longitude = parseFloat(message.match(/Долгота: (.*?),/)[1]);
        var latitude = parseFloat(message.match(/Широта: (.*?)$/)[1]);
        messageElement.innerHTML = `<iframe src="https://www.openstreetmap.org/export/embed.html?bbox=${longitude}%2C${latitude}&layer=mapnik&marker=${latitude},${longitude}" width="100%" height="300" frameborder="0" scrolling="no"></iframe>`;
    }
    if (type == 1) {
        //image = btoa(message);
        //newPath = message.replace('C:\\fakepath\\', '../../Фоточки/');
        
        messageElement.innerHTML = `<img src="../../Фоточки/${message}" width="100%" height="300" frameborder="0" scrolling="no"></img>`;
    }
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
                    displayMessage(element.text, "message-my", element.typeMessage);
                } else {
                    displayMessage(element.text, "message-other", element.typeMessage);
                }
            });

            const newPosition = container.scrollHeight - nowPosition;

            if (count == null) {
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
chatfield.addEventListener('scroll', function () {
    if (chatfield.scrollTop === 0) {
        console.log('Пользователь прокрутил содержимое .chatfield вверх');

        var history = document.querySelector('.history');
        var divHistory = history.getElementsByTagName('div');
        var divCountH = divHistory.length;

        var socket = document.querySelector('.web-socket');
        var divSocket = socket.getElementsByTagName('div');
        var divCountS = divSocket.length;

        console.log(divCountH + divCountS);

        getHistory(`http://localhost:5294/api/Messager/history?idToUser=${to}&count=${divCountH + divCountS + 50}`, token);
    }
});


// Select
document.getElementById("messageType").addEventListener("change", handleMessageSelection);
function handleMessageSelection() {
    var selectElement = document.getElementById("messageType");
    var fileInputElement = document.getElementById("fileInput");
    var selectedValue = selectElement.value;

    switch (selectedValue) {
        case "text":
            console.log("Сообщение выбрано");
            messageInput.value = '';
            type = 0;
            fileInputElement.style.display = "none";
            break;
        case "image":
            console.log("Изображение выбрано");
            messageInput.value = '';
            type = 1;
            fileInputElement.style.display = "block";
            break;
        case "location":
            console.log("Геолокация выбрана");
            messageInput.value = '';
            type = 2;
            fileInputElement.style.display = "none";
            findLocation();
            break;
        case "audio":
            console.log("Аудио выбрано");
            messageInput.value = '';
            type = 3;
            fileInputElement.style.display = "none";
            break;
        default:
            console.log("Неверная опция");
    }
}

function findLocation() {
    if (!navigator.geolocation) {
        console.log('Ваш браузер не дружит с геолокацией...');
    } else {
        navigator.geolocation.getCurrentPosition(success, error)
    }

    function success(position) {
        const { longitude, latitude } = position.coords
        messageInput.value = `Долгота: ${longitude}, Широта: ${latitude}`;
    }

    function error() {
        console.log('Не получается определить вашу геолокацию :(');
    }
}

function updateFilePath() {
    var fileInputElement = document.getElementById("fileInput");
    filePath = fileInputElement.files[0].name;
    //messageInput.value = fileInputElement.files[0];
    //fileInputElement.type = "hidden";
    
    messageInput.value = filePath;
    console.log(filePath);
}


let receivedChunks = [];
let imageData = '';

function receiveImageChunk(chunk) {
    receivedChunks.push(chunk);
    console.log(receivedChunks)
    imageData = receivedChunks.join('');
}

function reassembleImage() {
    const imageData = receivedChunks.join(''); // Объедините все части
    console.log(imageData);
    const byteCharacters = atob(imageData); // Декодируйте данные base64
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    
    const blob = new Blob([byteArray], { type: 'image/jpeg' }); // Создайте объект Blob
    const imageUrl = URL.createObjectURL(blob); // Создайте URL для Blob
    const imgElement = document.createElement('img');
    //imgElement.src = imageUrl;
    imgElement.src = imageData;
    
    // Отобразите восстановленное изображение
    document.body.appendChild(imgElement);
    console.log('data:image/png;base64,' + imgElement);
}