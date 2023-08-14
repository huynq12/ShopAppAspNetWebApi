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

async function displayBill() {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    cartItems.sort(function(a, b) {
        return a.productId - b.productId;
    });

    let grandTotal = 0;
    let cartItemsHtml = '';

    async function processCartItem(item) {
        try {
            const product = await getProductInfo(item.productId);
            var total = product.price * item.quantity;
            grandTotal += total;

            cartItemsHtml += `
                <div class="order-col">
                    <div>${item.quantity}x ${product.name}</div>
                    <div>${total}</div>
                </div>	
            `;
        } catch (error) {
            console.error(error);
        }
    }

    async function getProductInfo(productId) {
        return new Promise(function(resolve, reject) {
            $.ajax({
                url: 'https://localhost:7000/view-product/' + productId,
                type: 'GET',
                success: function(product) {
                    resolve(product);
                },
                error: function() {
                    reject();
                }
            });
        });
    }

    for (const item of cartItems) {
        await processCartItem(item);
    }

    $('#order-productList').html(cartItemsHtml);
    totalAmountHtml = '<p> ' + grandTotal + 'đ</p>';
    $('#totalAmount').html(totalAmountHtml);
}



$('#orderForm').submit(function (e) {
    e.preventDefault();
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
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
