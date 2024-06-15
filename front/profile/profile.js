var token = localStorage.getItem('token');

const url = `http://localhost:5294/profile`;
get(url, token)

async function get(url, token) {
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

        const date = new Date(data[0].dateBirth);
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        const formattedDate = date.toLocaleDateString('ru-RU', options);

        document.getElementById("username").textContent = data[0].login;
        document.getElementById("birthdate").textContent = formattedDate;
        //document.getElementById("profile-image").onerror= "this.src='./no-profile-min.png'";
        if (data[0].avatar != null && data[0].avatar != ''){
            document.getElementById("profile-image").src = data[0].avatar;
        }
      })
      .catch(error => {
        console.error('Ошибка', error);
      });
  }