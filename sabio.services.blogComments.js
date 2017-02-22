/// <reference path="sabio.services.blogComments.js" />
sabio.services.blogComments = sabio.services.blogComments || {};
sabio.services.blogComments.adminApiPrefix = "/api/v2/admin/blogs/";
sabio.services.blogComments.consumerApiPrefix = "/api/blogs/";

sabio.services.blogComments.insert = function (id, data, onSuccess, onError) {
    var url = sabio.services.blogComments.adminApiPrefix + id + "/comments";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: data
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };
    $.ajax(url, settings);

}

sabio.services.blogComments.get = function (onAjaxSuccess, onAjaxError) {
    var url = sabio.services.blogComments.consumerApiPrefix + "na";
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);

}

sabio.services.blogComments.update = function (id, data, onSuccess, onError) {

    var url = sabio.services.blogComments.adminApiPrefix + blogPostId + "/comments/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , data: data
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "PUT"
    };

    $.ajax(url, settings);

}

sabio.services.blogComments.delete = function (id, onSuccess, onError) {


    var url = "/api/blog-comments/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "DELETE"
    };

    $.ajax(url, settings);

}

sabio.services.blogComments.getById = function (id, onSuccess, onError) {

    var url = "/api/blog-comments/" + id + "/edit/";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);

}