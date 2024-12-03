// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var checkCreateUser = function () {
    if (document.getElementById('FirstName').value == "" || document.getElementById('LastName').value == "" ||
        document.getElementById('Password').value == "" || document.getElementById('ConfirmPassword').value =="") {
        document.getElementById('Submit').disabled = true;
    }
    else if (/\w+@[a-z]+.[a-z]+/.test(document.getElementById('Email').value) == false) {
        document.getElementById('Submit').disabled = true;
    }
    else if (document.getElementById('Password').value.length < 8) {
        document.getElementById('Submit').disabled = true;
    }
    else if (document.getElementById('Password').value !=
        document.getElementById('ConfirmPassword').value) {
        document.getElementById('Submit').disabled = true;
    }
    else {
        document.getElementById('Submit').disabled = false;
    }
}