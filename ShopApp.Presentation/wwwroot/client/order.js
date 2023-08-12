
$(document).ready(function(){
    $.ajaxSetup({
        beforeSend: function (xhr) {
            // Get token from localStorage
            var token = localStorage.getItem('token');

            if (token) {
                // Attach token to the Authorization header
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    displayBill()
})
function displayBill (){
    let productBill = '';
    let total=0
    let grandTotal = 0
    
    for (let i = 0;i < cartItems.length; i++) {
        let item = cartItems[i]
        $.ajax({
            url:'https://localhost:7000/view-product/'+item.productId,
            type:'GET',
            success:function(product){
                total = product.price*item.quantity
                productBill = `
                <div class="order-col">
                    <div>${item.quantity}x ${product.name}</div>
                    <div>${product.price}</div>
                </div>				
                `
                $('#order-products').append(productBill)
                grandTotal += total
                if(i==cartItems.length-1){
                    $('#totalAmount').html(grandTotal+'đ')
                }
            }
        })
            
        
        
    }
}

$('#orderForm').submit(function (e) {
    e.preventDefault();
    
    const orderData = {
        fullName : $('#order-fullName').val(),
        address : $('#order-address').val(),
        phoneNumber : $('#order-phoneNumber').val(),
        cartItems : cartItems
    }
    $.ajax({
        url: 'https://localhost:7000/place-order',
        type: 'POST',
        data:JSON.stringify(orderData),
        dataType: 'json',
        contentType: 'application/json',
        success: function () {
            console.log('Order successfully!');
            for (var i = cartItems.length - 1; i >= 0; i--) {
                    cartItems.splice(i, 1);
            }
            localStorage.setItem('cartItems', JSON.stringify(cartItems));
            
        },
        error: function (error) {
            console.log('Order failed. Please check your credentials.');
        }
    });
})
