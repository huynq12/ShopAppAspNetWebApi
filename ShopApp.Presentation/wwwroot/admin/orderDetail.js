const baseUrl = "https://localhost:7000"
$(document).ready(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var token = localStorage.getItem('token');

            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    var urlParams = new URLSearchParams(window.location.search);
    var orderId = urlParams.get('id');
    viewOrder(orderId)
});
function viewOrder(orderId){
    var orderDetailHtml = ''
    $.ajax({
        url: baseUrl +'/order/'+orderId,
        type:'GET',
        success:function(res){
            //console.log(res)
            $('#admin-order-orderId').val(orderId)
            $('#admin-order-fullName').val(res.userName)
            $('#admin-order-address').val(res.address)
            $('#admin-order-date').val(res.orderDate)
            $('#admin-order-phoneNumber').val(res.phoneNumber)
            $('#admin-order-status').val(res.status)
            $('#admin-order-totalAmount').val(res.totalAmount)
            var list = res.orderDetails.$values
            //console.log(list)
            for(let i = 0; i < list.length;i++){
                orderDetailHtml += '<tr>'
                orderDetailHtml += '<td>'+(i+1)+'</td>'
                orderDetailHtml += '<td>'+list[i].id+'</td>'
                orderDetailHtml += '<td>'+list[i].productName+'</td>'
                orderDetailHtml += '<td>'+list[i].price+'</td>'
                orderDetailHtml += '<td>'+list[i].amount+'</td>'
                orderDetailHtml += '<td>'+list[i].price*list[i].amount+'</td>'
                orderDetailHtml += '</tr>';
            }
            $('#admin-orderDetail').html(orderDetailHtml)
            var submitOrder = '';
            if(res.status == 'New'){
                submitOrder += '<a class="btn btn-secondary form-group mt-3" onclick=processOrder('+orderId+')>Process</a>';
            }
            if(res.status === 'Processing'){
                submitOrder += '<a class="btn btn-info form-group mt-3" onclick=shippedOrder('+orderId+')>Shipped</a>';
            }
            $('#admin-formFooter').append(submitOrder)
        }
    })
}

function processOrder(id){
    var orderData = {
        orderId : id,
        status : 'Processing'
    }
    console.log(orderData)
    if(confirm("Process this order?")){
        $.ajax({
            url :baseUrl + '/update-order-admin',
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


function shippedOrder(id){
    var orderData = {
        orderId : id,
        status : 'Shipped'
    }
    console.log(orderData)
    if(confirm("Are you sure this order has been shipped?")){
        $.ajax({
            url :baseUrl + '/update-order-admin',
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

