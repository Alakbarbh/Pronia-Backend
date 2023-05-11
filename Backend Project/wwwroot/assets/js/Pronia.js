$(document).ready(function () {

    ////get products by category  on click category
    $(document).on("click", ".category", function (e) {

        e.preventDefault();
        let colorId = $(this).attr("data-id");
        let parent = $(".product-list")
        $.ajax({

            url: `shop/GetProductsByCategory?id=${colorId}`,
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })


       
    })


    //get all products by category  on click All
    $(document).on("click", ".allproducts", function (e) {

        e.preventDefault();
        let parent = $(".product-list")
        $.ajax({

            url: "shop/GetAllProducts",
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })



    })

      //get products by color  on click color
    $(document).on("click", ".color", function (e) {

        e.preventDefault();
        let colorId = $(this).attr("data-id");
        let parent = $(".product-list")
        $.ajax({

            url: `shop/GetProductsByColor?id=${colorId}`,
            type: "Get",

            success: function (res) {
                $(parent).html(res);
            }
        })



    })



    //get all products by color  on click All
    $(document).on("click", ".allColors", function (e) {

        e.preventDefault();
        let parent = $(".product-list")
        $.ajax({

            url: "shop/GetAllProducts",
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
        let url = `/shop/mainsearch?searchText=${value}`;

        window.location.assign(url);


    })


    //get products by tag  on click tag
    $(document).on("click", ".tag", function (e) {

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