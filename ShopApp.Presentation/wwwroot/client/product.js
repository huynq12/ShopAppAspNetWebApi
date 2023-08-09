const baseUrl = 'https://localhost:7000'
$(document).ready(function () {
    //var cardHtml = ''
    $.ajax({
        url: baseUrl + '/products',
        type: 'GET',
        success: function (res) {
            for (let item of res.data.$values) {
                var cardHtml = `
                    <div class="col-md-4">
                        <div class="card mb-4">
                            <img class="card-img-top" src="./img/new-pictures/laptop1.jpg" alt="">
                            <div class="card-body">
                                <h4 class="card-title">${item.name}</h5>
                                <p class="card-text">${item.description}</p>
                                <p class="card-text">${item.price}đ</p>
                            </div>
                        </div>
                    </div>
                `;
                
                $("#product-list").append(cardHtml);
            }
            
        }
    })
})