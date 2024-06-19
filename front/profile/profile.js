var token = localStorage.getItem('token');

const urlParams = new URLSearchParams(window.location.search);
const id = urlParams.get('id');

if (id == null || id == undefined){
  get(`http://localhost:5294/profile`, token)
} else {
  document.getElementById("editProfileButton").style.display = "none";
  get(`http://localhost:5294/profile?id=${id}`)
  const profileImage = document.getElementById("profile-image");
  const newLink = `<a href="../chat/chat.html?&To=${id}" class="link buttons" style="margin-right: 0; margin-top: 15px; background-color: rgb(63, 63, 63);">Написать</a>`
  profileImage.insertAdjacentHTML('afterend', newLink);
}

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
          const pathParts = data[0].avatar.split('\\');
          const photoIndex = pathParts.indexOf('Фоточки');
          const relativePath = '../../'.repeat(pathParts.length - photoIndex - 1) + pathParts.slice(photoIndex).join('/');
          document.getElementById("profile-image").src = relativePath;
          console.log(relativePath);
        }
      })
      .catch(error => {
        console.error('Ошибка', error);
      });
  }