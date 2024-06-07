var token = localStorage.getItem('token');
const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', hour12: false };

const url = `https://localhost:44305/Message`;
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
        data.sort((a, b) => {
          if (a.isCheked !== b.isCheked) {
              return a.isCheked ? -1 : 1;
          } else {
              return new Date(b.dateTimeLastLetter) - new Date(a.dateTimeLastLetter);
          }
      });
        items(data);
      })
      .catch(error => {
        console.error('Ошибка', error);
      });
  }

  async function items(data){
    let codeString = '';
    data.forEach(item => {
      const date = new Date(item.dateTimeLastLetter);
      codeString += `
        <div class="messager">
            <a href="#" class="email-container">
                <img src="${item.urlAvatar}" alt="Avatar" class="avatar">
                <div>
                    <div class="name">${item.nameUser}</div>
                    
                    <div class="date">${date.toLocaleString(undefined, options)}</div>
                </div>
                ${item.isCheked ? '<div class="dot"></div>' : ''}
            </a>
        </div>
        `;
    });
    
    // Select the container element and set its innerHTML to the generated code
    const containerElement = document.querySelector('.container');
    containerElement.innerHTML = codeString;
  }