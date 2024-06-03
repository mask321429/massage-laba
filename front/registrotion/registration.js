async function registerPost(data) {
  const url = 'https://mis-api.kreosoft.space/api/doctor/register';
  return fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  })
    .then(response => response.json())
    .then(result => {
      console.log(result);
      const errorMessage = document.getElementById('errorMessage');
      errorMessage.textContent = '';

      if (result.message) {
        errorMessage.textContent = result.message;
        console.log(result.message);
      }

      if (result.title) {
        errorMessage.textContent = result.title;
        console.log(result.title);
      }
    
      if (result.token) {
        token = result.token;
        localStorage.setItem('token', token);
        window.location.href = '../mainWithAutorization/main.html';
      }
    })
    .catch(error => {
      console.error('Ошибка', error);
      const errorMessage = document.getElementById('errorMessage');
      errorMessage.textContent = 'Произошла ошибка при регистрации. Пожалуйста, попробуйте еще раз.';
    });
}

const form = document.querySelector('form');
if (form) {
  form.addEventListener('submit', function (event) {
    event.preventDefault();

    const login = document.getElementById('loginInput').value;
    const password = document.getElementById('passwordInput').value;
    const file = document.getElementById('fileInput').value;
    const date = document.getElementById('dateInput').value;

    const data = {
      login: login,
      password: password,
      date: date,
      file: file,
    };

    console.log(data);

    registerPost(data);
  });
}
