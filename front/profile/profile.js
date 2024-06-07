var token = localStorage.getItem('token');

const url = `https://localhost:44305/profile`;
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

        const date = new Date(data.dateBirth);
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        const formattedDate = date.toLocaleDateString('ru-RU', options);

        document.getElementById("username").textContent = data.login;
        document.getElementById("birthdate").textContent = formattedDate;
        //document.getElementById("profile-image").onerror= "this.src='./no-profile-min.png'";
        if (data.avatar != null && data.avatar != ''){
            document.getElementById("profile-image").src = data.avatar;
        }
      })
      .catch(error => {
        console.error('Ошибка', error);
      });
  }