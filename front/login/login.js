const form = document.querySelector('form');
if (form) {
  form.addEventListener('submit', function (event) {
    event.preventDefault(); // Предотвращает стандартное поведение браузера, связанное с отправкой формы, чтобы избежать перезагрузки страницы.ы
    
    var login = document.getElementById('loginInput').value;
    var password = document.getElementById('passwordInput').value;

    fetch('https://localhost:44305/api/Auth/login', {
    method: 'POST',
    headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json'
    },
    body: JSON.stringify({
    login: login,
    password: password
    })
    })
    //Если ответ успешен, данные ответа преобразуются в JSON и обрабатываются
    .then(response => {
    if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
    })
    .then(data => {
    // Обрабатываем данные ответа здесь
    console.log(data);
    if (data.token) {
        token = data.token;
        localStorage.setItem('token', token);
        window.location.href = '../mainWithAutorization/main.html';
      } else {
        alert('Неверный логин или пароль');
      }

    // Сохраняем токен в localStorage
    localStorage.setItem('token', data.token);
    })
    .catch(error => {
    // Обрабатываем ошибки здесь
    alert('Неверный логин или пароль');
    console.error('Ошибка:', error.message);
    });
    });
  }