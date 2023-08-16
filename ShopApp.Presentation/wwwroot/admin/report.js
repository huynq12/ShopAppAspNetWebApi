baseUrl = 'https://localhost:7000'
$(document).ready(function(){
    topCategories()
    topSoldProducts()
    topReviewProducts()
})
function topCategories(){
    var html = ''
    $.ajax({
        url:baseUrl + '/report/topCategories',
        type:'GET',
        success:function(res){
            //console.log(res)
            var list1 = res.items.$values
            var list2 = res.listCount.$values
            for(let i = 0; i< res.items.$values.length;i++){
                html = `
                    <tr>
                        <td>${i+1}</td>
                        <td>${list1[i].name}</td>
                        <td>${list2[i]}</td>
                    </tr>
                `
                $('#topCategoryList').append(html)
            }
            
        }
    })
}

function topSoldProducts(){
    var html = ''
    $.ajax({
        url:baseUrl + '/report/topSoldProducts',
        type:'GET',
        success:function(res){
            //console.log(res)
            var list1 = res.items.$values
            var list2 = res.listCount.$values
            for(let i = 0; i< res.items.$values.length;i++){
                html = `
                    <tr>
                        <td>${i+1}</td>
                        <td>${list1[i].name}</td>
                        <td>${list2[i]}</td>
                    </tr>
                `
                $('#topSoldProducts').append(html)
            }
            
        }
    })
}

function topReviewProducts(){
    var html = ''
    $.ajax({
        url:baseUrl + '/report/topReviewProducts',
        type:'GET',
        success:function(res){
            //console.log(res)
            var list1 = res.items.$values
            var list2 = res.listCount.$values
            for(let i = 0; i< res.items.$values.length;i++){
                html = `
                    <tr>
                        <td>${i+1}</td>
                        <td>${list1[i].name}</td>
                        <td>${list2[i]}</td>
                    </tr>
                `
                $('#topReviewProducts').append(html)
            }
            
        }
    })
}
