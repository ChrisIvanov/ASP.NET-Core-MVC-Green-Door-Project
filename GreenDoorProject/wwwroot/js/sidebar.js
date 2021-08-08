const button = document.querySelector('button');

button.addEventListener('click', _ => {
    document.getElementById('sidebar').classList.toggle('collapsed');
})