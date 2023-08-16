
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
    displayListProduct()
    //getCategoryOptions()
    
})
function displayListProduct(){
    var categoryId = $('#categoryId').val()
    $.ajax({
        url: baseUrl + '/products',
        type: 'GET',
        //contentType:'application/json',
        //data:JSON.stringify(categoryId),
        success: function (res) {
            for (let item of res.data.$values) {
                var cardHtml = `
                    <div class="col-md-3" >
                        <a href="/product?id=${item.id}">
                        <div class="card mb-4" id="btnCard" data-product-id="${item.id}">
                            <input type="hidden" id="productDetailsId" value="${item.id}"/>
                            <img class="card-img-top" src="${item.imageUrl}" alt="laptop" style="width:100%;height:270px">
                            <hr/>
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
}

function getCategoryOptions() {
    $.ajax({
        url: baseUrl + '/categories',
        type: 'GET',
        dataType: 'JSON',
        success: function (data) {
            const categorySelects = $('#categoryId');
            var list = data.$values
            for (var i = 0; i < list.length; i++) {
                categorySelects.append($('<option>', {
                    value: list[i].id,
                    text: list[i].name
                }));
            };
        },
        error: function (error) {
            console.error(JSON.stringify(error));
        }
    });
}

