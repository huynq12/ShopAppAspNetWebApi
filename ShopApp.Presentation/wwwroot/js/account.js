$('#loginForm').submit(function (e) {
    e.preventDefault();
    
    const loginData = {
        email : $('#login-email').val(),
        password : $('#login-password').val()
    }
    $.ajax({
        url: 'https://localhost:7000/login',
        type: 'POST',
        data:JSON.stringify(loginData),
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            localStorage.setItem('token', response.token);
            console.log(response)
            console.log('Login successful!');
            window.location = "https://localhost:6999"
        },
        error: function (error) {
            console.log('Login failed. Please check your credentials.');
        }
    });
})

$('#registerForm').submit(function (e) {
    e.preventDefault();
    
    const registerData = {
        email : $('#register-email').val(),
        password : $('#register-password').val(),
        passwordConfirm:$('#register-password-confirm').val()
    }
    $.ajax({
        url: 'https://localhost:7000/register',
        type: 'POST',
        data:JSON.stringify(registerData),
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            //localStorage.setItem('token', response.token);
            console.log(response)
            console.log('register successful!');
            alert("register successfully")
            window.location = "https://localhost:6999/login"
        },
        error: function (error) {
            console.log('Login failed. Please check your credentials.');
        }
    });
})