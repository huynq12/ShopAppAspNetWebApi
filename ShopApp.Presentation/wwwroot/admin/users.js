const baseUrl = 'https://localhost:7000'
$(document).ready(function(){
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var token = localStorage.getItem('token');
            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    displayUsers()
})

function displayUsers(){
    var str = ''
    $.ajax({
        url:baseUrl + '/users',
        type:'GET',
        success:function(res){
            console.log(res)
            for(let item of res.$values){
                str += `
                    <tr>
                        <td>${item.email}</td>
                        <td>${item.fullName}</td>
                        <td>${item.phoneNumber}</td>
                        <td>${item.address}</td>
                    </tr>
                `
            } 
            $('#listUser').html(str)  

        }
    })
}

