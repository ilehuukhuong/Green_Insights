// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let btn = document.querySelector('#btn');
let sidebar = document.querySelector('.sidebar');
let srcBtn = document.querySelector('bx-search-alt');

btn.onclick = function () {
    sidebar.classList.toggle('active');
}

srcBtn.onclick = function () {
    sidebar.classList.toggle('active');
}
