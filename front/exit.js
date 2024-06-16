document.querySelector('.link.exit').addEventListener('click', function() {
    localStorage.removeItem('token');
});