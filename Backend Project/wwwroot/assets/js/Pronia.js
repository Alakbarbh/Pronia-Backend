$(document).ready(function () {

    $(document).on("click", ".category", function (e) {
        e.preventDefault();
        let categoryId = $(this).attr("data-id");
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductByCategory?id=${categoryId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })



    $(document).on("click", ".all-product", function (e) {
        e.preventDefault();
        let parent = $(".product-list");

        $.ajax({
            url: "shop/GetAllProduct",
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })


    $(document).on("click", ".product-color", function (e) {
        e.preventDefault();
        let colorId = $(this).attr("data-id")
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductByColor?id=${colorId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })


    $(document).on("click", ".all-color", function (e) {
        e.preventDefault();
        let parent = $(".product-list");

        $.ajax({
            url: "shop/GetAllProductByColor",
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })


    $(document).on("click", ".select", function (e) {
        e.preventDefault();
        let colorId = $(this).attr("data-id")
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductFilteredByPrice?id=${colorId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })



    //search
    $(document).on("submit", ".hm-searchbox", function (e) {
        e.preventDefault();
        let value = $(".input-search").val();
        let url = `/shop/MainSearch?searchText=${value}`;

        window.location.assign(url);

    })



    //SEARCH WITH li

    $(document).on("keyup", ".input-field", function () {
        debugger
        $("#search-list li").slice(1).remove();
        let value = $(".input-field").val();

        $.ajax({

            url: `shop/search?searchText=${value}`,

            type: "Get",

            success: function (res) {
                console.log(res)
                $("#search-list").append(res);
            }



        })
    })



    $(document).on("click", ".product-tag", function (e) {
        e.preventDefault();
        let tagId = $(this).attr("data-id")
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductsByTag?id=${tagId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })






    //blog



    $(document).on("click", ".category", function (e) {
        e.preventDefault();
        let categoryId = $(this).attr("data-id");
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductByCategory?id=${categoryId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })



    $(document).on("click", ".all-product", function (e) {
        e.preventDefault();
        let parent = $(".product-list");

        $.ajax({
            url: "shop/GetAllProduct",
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })



    
})