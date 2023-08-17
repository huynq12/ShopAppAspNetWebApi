const baseUrl = 'https://localhost:7000';   
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
            //console.log(res)
            for(let item of res.$values){
                str += '<tr><td>'+item.userName+'</td>'
                str += '<td>'+item.fullName+'</td>'        
                str += '<td>'+item.phoneNumber+'</td>'
                str += '<td>'+item.address+'</td>'
                str += '<td><button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#user-orders" onclick=getUserName("'+item.userName+'")>View orders</button></td></tr>'
            } 
            $('#listUser').html(str)  
        }
    })
}
function getUserName(email){
    $('#user-email').val(email)
    displayUserOrders()
}

function displayUserOrders(){
    var email = $('#user-email').val()
    var html = ''
    $.ajax({
        url:baseUrl + '/orders-by/'+ email,
        type:'GET',
        success:function(res){
            console.log(res)
            for(let item of res.$values){
                html += '<tr><td>' + item.id + '</td>'
                html += '<td>' + item.userName + '</td>'
                html += '<td>' + item.user + '</td>'
                html += '<td>' + item.phoneNumber + '</td>'
                html += '<td>' + item.address + '</td>'
                html += '<td>'+item.orderDate+'</td>'
                html += '<td>'+item.status+'</td>'
                html += '<td><a class="btn btn-info">View</a></td>'
                // if(item.status == 'New')
                //     html += ' | <a class="btn btn-success">Process</a>'
                // if(item.status == 'Processing')
                //     html += ' | <a class="btn btn-warning">Shipped</a>'
                // html += '</td>'
            }
            $('#listUserOrders').html(html)
        }

    })
}



