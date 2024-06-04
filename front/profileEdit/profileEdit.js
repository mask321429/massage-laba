var token = localStorage.getItem('token');

async function profilePut(data) {
  const url = 'https://localhost:44305/api/User/profile';
  const formData = new FormData();
  formData.append('Login', data.login);
  formData.append('BirthDate', data.date);
  formData.append('Avatar', data.file);
  
  const requestOptions = {
    method: 'PUT',
    headers: {
      'accept': '*/*',
      "Authorization": `Bearer ${token}`
    },
    body: formData
  };

  return fetch(url, requestOptions)
    .then(result => {
      console.log(result);
      const errorMessage = document.getElementById('errorMessage');
      errorMessage.textContent = '';
      
      if (result.error) {
        errorMessage.textContent = result.error;
        console.log(result.error);
      }
      
      if (result.status == "200") {
        errorMessage.textContent = result.message;
        window.location.href = '../profile/profile.html';
      }
      
      if (result.errors) {
        for (let key in result.errors) {
          if (result.errors.hasOwnProperty(key)) {
            errorMessage.textContent += `${key}: ${result.errors[key].join(' ')} \n`;
            console.log(`${key}: ${result.errors[key].join(' ')}`);
          }
        }
      }
    })
    .catch(error => {
      console.error('Error', error);
      const errorMessage = document.getElementById('errorMessage');
      errorMessage.textContent = 'Ошибка';
    });
}

const form = document.querySelector('form');
if (form) {
  form.addEventListener('submit', function (event) {
    event.preventDefault();
    const login = document.getElementById('loginInput').value;
    const fileInput = document.getElementById('fileInput');
    const date = document.getElementById('dateInput').value;
    
    const file = fileInput.files[0];

    const data = {
      login: login,
      date: date,
      file: file
    };

    console.log(data);
    profilePut(data);
  });
}
