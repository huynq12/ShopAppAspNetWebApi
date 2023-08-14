const baseUrl = 'https://localhost:7000'
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
    displayCategories()
})

function displayCategories(){
    var str = ''
    $.ajax({
        url:baseUrl + '/categories',
        type:'GET',
        success:function(res){
            console.log(res)
            for(let item of res.$values){
                str += `
                    <tr>
                        <td>${item.id}</td>
                        <td>${item.name}</td>
                        <td>${item.description}</td>
                        <td><a class="btn btn-warning" onclick=editCategory(${item.id})>Edit</a></td>
                    </tr>
                `
            } 
            $('#listCategory').html(str)  

        }
    })
}

function addNewCategory(){
    var categoryData = {
        name : $('#category-create-name').val(),
        description : $('#category-create-description').val()
    }
    $.ajax({
        url:baseUrl + '/create-category',
        type:'POST',
        contentType : 'application/json',
        data:JSON.stringify(categoryData),
        success:function(){
            $('#category-create').modal('hide')
            displayCategories()
        }
    })
}
function editCategory(id){

}
function updateCategory(){
    edit
}