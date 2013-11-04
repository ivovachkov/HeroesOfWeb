(function () {
    function performGetRequest(url, headers) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                type: "GET",
                dataType: "json",
                headers: headers,
                contentType: "application/json",
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
        return promise;
    }

    function performPostRequest(url, requestData, headers) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                headers: headers,
                contentType: "application/json",
                data: JSON.stringify(requestData),
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
        return promise;
    }

    function performPutRequest(url, requestData, headers) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                type: "PUT",
                dataType: "json",
                headers: headers,
                contentType: "application/json",
                data: JSON.stringify(requestData),
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
        return promise;
    }
    window.request = {
        getJSON: performGetRequest,
        postJSON: performPostRequest,
        putJSON: performPutRequest
    }
}());