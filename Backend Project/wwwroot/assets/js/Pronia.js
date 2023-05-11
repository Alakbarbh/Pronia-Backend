$(document).ready(function () {

    ////get products by category  on click category
    $(document).on("click", ".category", function (e) {

        e.preventDefault();
        let colorId = $(this).attr("data-id");
        let parent = $(".product-list")
        $.ajax({

            url: `shop/GetProductByCategory?id=${colorId}`,
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })


       
    })


    //get all products by category  on click All
    $(document).on("click", ".all-product", function (e) {

        e.preventDefault();
        let parent = $(".product-list")
        $.ajax({

            url: "shop/GetAllProduct",
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })



    })

      //get products by color  on click color
    $(document).on("click", ".product-color", function (e) {

        e.preventDefault();
        let colorId = $(this).attr("data-id");
        let parent = $(".product-list")
        $.ajax({

            url: `shop/GetProductByColor?id=${colorId}`,
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })



    })


    //get all products by color  on click All
    $(document).on("click", ".all-color", function (e) {

        e.preventDefault();
        let parent = $(".product-list")
        $.ajax({

            url: "shop/GetAllProductByColor",
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })



    })




    //SEARCH WITH li

    $(document).on("keyup", ".input-field", function () {
        $("#search-list li").slice(1).remove();
        let value = $(".input-field").val();  
       
        $.ajax({  

            url: `shop/search?searchText=${value}`,  

            type: "Get",

            success: function (res) {
               
                $("#search-list").append(res);  
            }



        })

    })



    //MAIN SEARCH

    $(document).on("submit", ".hm-searchbox", function (e) {
        e.preventDefault();
        let value = $(".input-search").val();
        let url = `/shop/MainSearch?searchText=${value}`;

        window.location.assign(url);

    })


    //get products by tag  on click tag
    $(document).on("click", ".product-tag", function (e) {

        e.preventDefault();
        let tagId = $(this).attr("data-id");
        let parent = $(".product-grid-view")
        $.ajax({

            url: `shop/GetProductsByTag?id=${tagId}`,
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })



    })




    //BLOGS




})