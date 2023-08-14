
const baseUrl = 'https://localhost:7000'
$(document).ready(function () {
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
    $.ajax({
        url: baseUrl + '/products',
        type: 'GET',
        success: function (res) {
            for (let item of res.data.$values) {
                var cardHtml = `
                    <div class="col-md-3" >
                        <a href="/product?id=${item.id}">
                        <div class="card mb-4" id="btnCard" data-product-id="${item.id}">
                            <input type="hidden" id="productDetailsId" value="${item.id}"/>
                            <img class="card-img-top" src="./img/new-pictures/laptop1.jpg" alt="">
                            <div class="card-body">
                                <h4 class="card-title">${item.name}</h5>
                                <p class="card-text">${item.description}</p>
                                <p class="card-text">${item.price}đ</p>
                            </div>
                        </div>
                       </a>
                    </div>
                `;

                $("#product-list").append(cardHtml);
            }

        }
    })
})


