/// <reference path="../libs/_references.js" />
window.persisters = (function () {
    function saveUserData(userData) {
        if (userData.sessionKey && userData.displayName) {
            localStorage.setItem('sessionKey', userData.sessionKey);
            localStorage.setItem('displayName', userData.displayName);
            localStorage.setItem('gold', userData.gold);
        }
    }
    function clearUserData() {
        localStorage.removeItem('sessionKey');
        localStorage.removeItem('displayName');
        localStorage.removeItem('gold');
    }
    function getSessionKey() {
        return localStorage.getItem('sessionKey');
    }

    var MainPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
            this.users = new UserPersister(apiUrl + 'users/');
            this.heros = new HeroPersister(apiUrl + 'hero/');
            this.monsters = new MonsterPersister(apiUrl + 'monsters/');
            this.items = new ItemsPersister(apiUrl + 'items/');
        },
        isUserLoggedIn: function () {
            return !!(getSessionKey());
        }
    });

    var UserPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
        },
        register: function (username, displayName, password) {
            var url = this.apiUrl + 'register';
            user = {
                username: username,
                displayName: username, //TODO implement with username
                authCode: CryptoJS.SHA1(password).toString()
            };
            return request.postJSON(url, user)
                .then(function (data) {
                    saveUserData(data);
                    return data.displayName;
                });
        },
        login: function (username, password) {
            var url = this.apiUrl + 'login';
            user = {
                username: username,
                authCode: CryptoJS.SHA1(password).toString()
            };
            return request.postJSON(url, user)
                .then(function (data) {
                    saveUserData(data);
                    return data.displayName;
                });
        },
        logout: function () {
            var url = this.apiUrl + 'logout';
            if (!getSessionKey()) {
                //TODO throw error
            }
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            clearUserData();
            localStorage.removeItem('hero');
            return request.putJSON(url, {}, headers)
                .then(function (data) {
                    return data;
                });
        },
        update: function (gold) {
            var url = this.apiUrl + "update";
            var headers = {
                'X-sessionKey': getSessionKey()
            };

            var obj = {};
            obj.gold = gold;
            return request.putJSON(url, obj, headers)
            .then(function (data) {
                return data;
            })
        },
        currentUser: function () {
            return localStorage.getItem('displayName');
        },
        setAvatar: function (avatarUrl) {
            var url = this.apiUrl + 'avatar';
            user = {
                avatarUrl: avatarUrl,
            };
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.putJSON(url, user, headers);
        },
        getAll: function () {
            var url = this.apiUrl;
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.getJSON(url, headers);
        }
    });

    var HeroPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
        },
        create: function (heroData) {
            var url = this.apiUrl;
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.postJSON(url, heroData, headers);
        },
        getAll: function () {
            var url = this.apiUrl + 'getAll';
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.getJSON(url, headers);
        },
        getById: function (id) {
            var url = this.apiUrl + 'getById/' + id;
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.getJSON(url, headers);
        },
        getUsersHero: function () {
            var url = this.apiUrl + 'getUsersHero';
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.getJSON(url, headers)
                .then(function (data) {
                    localStorage.setItem('hero', JSON.stringify(data[0]));
                    return data;
                });
        },
        update: function (heroData) {
            debugger
            var url = this.apiUrl;
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            localStorage.setItem('hero', JSON.stringify(heroData));
            return request.putJSON(url, heroData, headers);
        }
    });

    var MonsterPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
        },
        get: function () {
            var url = this.apiUrl;
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.getJSON(url, headers);
        },
        getById: function (id) {
            var url = this.apiUrl + id;
            var headers = {
                'X-sessionKey': getSessionKey()
            };
            return request.getJSON(url, headers);
        }
    });

    var ItemsPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
        },
        get: function () {
            var url = this.apiUrl;
            var headers = {
                "X-sessionKey": getSessionKey()
            };
            return request.getJSON(url, headers);
        },
        getById: function (id) {
            var url = this.apiUrl + id;
            var headers = {
                "X-sessionKey": getSessionKey()
            };
            return request.getJSON(url, headers);
        },
        getByCategory: function (category) {
            var url = this.apiUrl + '?category=' + category;
            var headers = {
                "X-sessionKey": getSessionKey()
            };
            return request.getJSON(url, headers);
        }
    });

    return {
        get: function (apiUrl) {
            return new MainPersister(apiUrl);
        }
    };
}());