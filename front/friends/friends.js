var token = localStorage.getItem('token');
const options = { year: 'numeric', month: '2-digit', day: '2-digit' };

const url = `http://localhost:5294/profile?name=s`;
//get(url, token)

async function get(url) {
    return fetch(url, {
      method: 'GET',
    })
      .then(response => response.json())
      .then(data => {
        console.log(url);
        console.log(data);
        items(data);
      })
      .catch(error => {
        console.error('Ошибка', error);
      });
  }

  async function items(data){
    let codeString = '';
    data.forEach(item => {
      const date = new Date(item.dateBirth);
      console.log(item);
      const pathParts = item.avatar.split('\\');
      const photoIndex = pathParts.indexOf('Фоточки');
      const relativePath = '../../'.repeat(pathParts.length - photoIndex - 1) + pathParts.slice(photoIndex).join('/');
      codeString += `
        <div class="messager">
            <a href="#" class="users-container">
                <img src="${relativePath}" onerror="this.src='../profile/no-profile-min.png'" class="avatar">
                <div style="overflow: hidden;ъ">
                    <div class="name">${item.login}</div>
                    <div class="date">${date.toLocaleString(undefined, options)}</div>
                </div>
            </a>
        </div>
        `;
    });
    
    const containerElement = document.querySelector('.aaa');
    containerElement.innerHTML = codeString;
  }

  function searchPeople() {
    console.log(document.getElementById("textInput").value);
    if (document.getElementById("textInput").value != ''){
      get(`http://localhost:5294/profile?name=${encodeURIComponent(document.getElementById("textInput").value)}`)
    }
}