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
    displayListOrders()
})
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
                ordersHtml += '<td><a href="/order-detail?id='+item.id+'" class="btn btn-info" >View</a> | <a class="btn btn-success" onclick=completeOrder('+item.id+')>Complete</a> | <a class="btn btn-danger" onclick=cancelOrder('+item.id+')>Cancel</a></td>'
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

function completeOrder(id){
    var orderData = {
        orderId : id,
        status : 'success'
    }
    console.log(orderData)
    if(confirm("Do you want to complete this order?")){
        $.ajax({
            url :baseUrl + '/update-order',
            type: 'PUT',
            contentType: "application/JSON",
            data: JSON.stringify(orderData),
            success:function(){
                console.log('success')
                location.reload()
            },
            error: function () {
                alert("Error");
            }
        })
    }
    
}

function cancelOrder(id){
    var orderData = {
        orderId : id,
        status : 'cancel'
    }
    console.log(orderData)
    if(confirm("Do you want to cancel this order?")){
        $.ajax({
            url :baseUrl + '/update-order',
            type: 'PUT',
            contentType: "application/JSON",
            data: JSON.stringify(orderData),
            success:function(){
                console.log('success')
                location.reload()
            },
            error: function () {
                alert("Error");
            }
        })
    }
    
}