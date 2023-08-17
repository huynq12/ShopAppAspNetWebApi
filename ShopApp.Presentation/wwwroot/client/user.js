const baseUrl = 'https://localhost:7000'

$(document).ready(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var token = localStorage.getItem('token');

            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    getProfile()
    displayListOrders()
})
function getProfile(){
    var html = ''
    $.ajax({
        url: baseUrl + '/profile',
        type: 'GET',
        success:function(res){
            var greet = 'Wellcome '
            greet += res.fullName
            $('#greeter').html(greet)
            $('#user-edit-email').val(res.userName)
            $('#user-edit-fullName').val(res.fullName)
            $('#user-edit-phoneNumber').val(res.phoneNumber)
            $('#user-edit-address').val(res.address)
            //console.log(res)
            html += '<table class="table table-bordered"><tr><td>Email:</td><td>'+res.email+'</td></tr>'
            html += '<tr><td>Full name:</td><td>'
            html += res.fullName == null ? '' : res.fullName + '</td></tr>'
            html += '<tr><td>Phone number:</td><td>'
            html += res.phoneNumber == null ? '' : res.phoneNumber + '</td></tr>'
            html += '<tr><td>Address:</td><td>'
            html += res.address == null ? '' : res.address + '</td></tr>'
            html += '</table>'    
            html += '<button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#user-edit">Edit</button>'   
            $('#profile').html(html)
        }
    })
}

function updateProfile(){
    var userData = {
        fullName : $('#user-edit-fullName').val(),
        phoneNumber : $('#user-edit-phoneNumber').val(),
        address : $('#user-edit-address').val()
    }
    $.ajax({
        url:baseUrl + '/update-user',
        type:'PUT',
        contentType: "application/JSON",
        data: JSON.stringify(userData),
        success:function(){
            $('#user-edit').modal('hide')
            console.log('success')
            getProfile()
        },
        error: function () {
            alert("Error");
        }
    })
}

function displayListOrders() {
    var ordersHtml = ''
    $.ajax({
        url: baseUrl + '/user-orders',
        type: 'GET',
        success: function (res) {
            for (let item of res.$values) {
                ordersHtml += '<tr>'
                ordersHtml += '<td>' + item.id + '</td>'
                ordersHtml += '<td>' + item.userName + '</td>'
                ordersHtml += '<td>' + item.address + '</td>'
                ordersHtml += '<td>' + item.phoneNumber + '</td>'
                ordersHtml += '<td>' + item.orderDate + '</td>'
                ordersHtml += '<td>' + item.totalAmount + '</td>'
                ordersHtml += '<td>' + item.status + '</td>'
                ordersHtml += '<td><a href="/order-detail?id='+item.id+'" class="btn btn-info" >View</a></td>'
                // if(item.status == 'New' || item.status == 'Processing')
                //     ordersHtml += ' | <a class="btn btn-danger" onclick=cancelOrder('+item.id+')>Cancel</a></td>'
                // if(item.status == 'Shipped')
                //     ordersHtml += ' | <a class="btn btn-success" onclick=completeOrder('+item.id+')>Complete</a>'
                // | <a class="btn btn-success" onclick=completeOrder('+item.id+')>Complete</a> | <a class="btn btn-danger" onclick=cancelOrder('+item.id+')>Cancel</a></td>'
                ordersHtml += '</tr>'
            }
            $('#listOrders').html(ordersHtml)
        }
    })
}

function viewOrder(id){
    $.ajax({
        url: baseUrl +'/order/'+id,
        type:'GET',
        success:function(res){
            $('#orderDetail').modal('show')
            console.log(res)
        }
    })
}

