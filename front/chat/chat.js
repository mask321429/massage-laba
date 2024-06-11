var token = localStorage.getItem('token');
const urlParams = new URLSearchParams(window.location.search);

const from = urlParams.get('From');
const to = urlParams.get('To');

console.log('From:', from);
console.log('To:', to);