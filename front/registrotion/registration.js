async function registerPost(data) {
  const url = 'http://localhost:5294/api/Auth/register';

  const formData = new FormData();
  formData.append('Login', data.login);
  formData.append('Password', data.password);
  formData.append('BirthDate', data.date);
  formData.append('Avatar', data.file);

  return fetch(url, {
    method: 'POST',
    body: formData
  })
    .then(response => response.json())
    .then(result => {
      console.log(result);
      const errorMessage = document.getElementById('errorMessage');
      errorMessage.textContent = '';

       if (result.error) {
         errorMessage.textContent = result.error;
         console.log(result.error);
       }
       
      if (result.message) {
        errorMessage.textContent = result.message;
        console.log(result.message);
      }

       if (result.errors) {
        for (let key in result.errors) {
            if (result.errors.hasOwnProperty(key)) {
                errorMessage.textContent += `${key}: ${result.errors[key].join(' ')} \n`;
                console.log(`${key}: ${result.errors[key].join(' ')}`);
            }
        }
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
    const fileInput = document.getElementById('fileInput');
    const date = document.getElementById('dateInput').value;
    
    const file = fileInput.files[0];

    const data = {
      login: login,
      password: password,
      date: date,
      file: file
    };

    console.log(data);
    registerPost(data);
  });
}