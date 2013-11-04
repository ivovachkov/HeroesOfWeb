/// <reference path="../libs/_references.js" />
window.VMfactory = (function () {
    var data = persisters.get("/api");
    var player = {};
    var monster = {};
    function hero() {
        var heroStr = localStorage.getItem("hero");
        return JSON.parse(heroStr);
    }

    function getRand(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    function getLoginViewModel(successCallback) {
        var viewModel = {
            username: "gargamel",
            password: "gargamel",
            message: "",
            login: function () {
                //TODO verify data
                data.users.login(this.get('username'), this.get('password'))
                    .then(function (usr) {
                        if (successCallback) {
                            successCallback();
                        }
                    });
            },
            register: function () {
                //TODO verify data
                data.users.register(this.get('username'), this.get('displayName'), this.get('password'))
                .then(function (name) {
                    newHero = {
                        "name": name,
                        "maxHp": 250,
                        "hp": 200,
                        "maxMp": 150,
                        "mp": 100,
                        "magicAttack": 20,
                        "meleAttack": 20,
                        "magicDefense": 10,
                        "meleDefense": 10,
                        "experience": 0,
                        "level": 1
                    };
                    data.heros.create(newHero)
                    .then(function () {
                        if (successCallback) {
                            successCallback();
                        }
                    });
                });
            },
            setAvatar: function () {
                data.users;
            }
        };
        return kendo.observable(viewModel);
    }

    function getGameViewModel(successCallback) {
        var viewModel = {
            gold: localStorage.getItem("gold"),
            logout: function () {
                data.users.logout()
                .then(function () {
                    if (successCallback) {
                        successCallback();
                    }
                });
            }
        };
        return kendo.observable(viewModel);
    }

    function getActionsViewModel(successCallback) {
        var viewModel = {
            main: function () {
                successCallback("/main");
            },
            monster: function () {
                successCallback("/monster");
            },
            player: function () {
                successCallback("/PvP");
            },
            heal: function () {
                successCallback("/construction");
            },
            shop: function () {
                successCallback("/shop");
            }
        };
        return kendo.observable(viewModel);
    }

    function getHeroViewModel(successCallback, renderStats) {
        return data.heros.getUsersHero()
            .then(function (heroData) {
                //localStorage.setItem("hero", JSON.stringify(heroData[0]));
                var viewModel = {
                    name: heroData[0].name,
                    maxHp: heroData[0].maxHp,
                    hp: heroData[0].hp,
                    maxMp: heroData[0].maxMp,
                    mp: heroData[0].mp,
                    magicAttack: heroData[0].magicAttack,
                    meleAttack: heroData[0].meleAttack,
                    magicDefense: heroData[0].magicDefense,
                    meleDefense: heroData[0].meleDefense,
                    experience: heroData[0].experience,
                    level: heroData[0].level,
                    items: heroData[0].items,
                    imageUrl: "/Images/hero.png",
                    select: function (ev) {
                        var target = $(ev.currentTarget);
                        var id = parseInt(target.context.id, 10);
                        var item = _.find(this.items, function (itm) {
                            return itm.itemId == id;
                        });
                        if (item.itemCategory == 'consumable') {
                            if (item.hp + this.hp > this.maxHp) {
                                this.hp = this.maxHp;
                            }
                            else {
                                this.hp += item.hp;
                            }

                            if (item.mp + this.mp > this.maxMp) {
                                this.mp = this.maxMp;
                            }
                            else {
                                this.mp += item.mp;
                            }
                            $("#" + id).remove();
                            var updatedHero = this;
                            updatedHero.items.pop(item);

                            updatedHero.magicAttack += item.magicAttack;
                            updatedHero.meleAttack += item.meleAttack;
                            updatedHero.magicDefense += item.magicDefense;
                            updatedHero.meleDefense += item.meleDefense;
                            data.heros.update(updatedHero)
                                .then(function () {
                                    data.heros.getUsersHero()
                                    .then(function (heroData) {
                                        renderStats(heroData);
                                    })
                                })
                        }
                        else if (!target.hasClass('equiped')) {
                            $('#' + id).addClass('equiped');
                            this.maxHp += item.hp;
                            this.maxMp += item.mp;
                            this.hp += item.hp;
                            this.mp += item.mp;
                            this.magicAttack += item.magicAttack;
                            this.meleAttack += item.meleAttack;
                            this.magicDefense += item.magicDefense;
                            this.meleDefense += item.meleDefense;
                        }
                        else if (target.hasClass('equiped')) {
                            $('#' + id).removeClass('equiped');
                            this.maxHp -= item.hp;
                            this.maxMp -= item.mp;
                            this.hp -= item.hp;
                            this.mp -= item.mp;
                            this.magicAttack -= item.magicAttack;
                            this.meleAttack -= item.meleAttack;
                            this.magicDefense -= item.magicDefense;
                            this.meleDefense -= item.meleDefense;
                        }

                        if (renderStats) {
                            renderStats(this);
                        }
                    },
                    equip: function () {

                        if (successCallback) {
                            successCallback(this);
                        }
                    },
                    gotoMain: function () {
                        successCallback("/main");
                    },
                    gotoMonster: function () {
                        successCallback("/monster");
                    },
                    gotoPlayer: function () {
                        successCallback("/PvP");
                    },
                    gotoHeal: function () {
                        successCallback("/construction");
                    },
                    gotoShop: function () {
                        successCallback("/shop");
                    }
                };
                return kendo.observable(viewModel);
            });
    }

    function getShopViewModel(successCallback) {
        return data.items.get()
            .then(function (data) {
                var viewModel = {
                    items: data,
                    listener: function (ev) {
                        var target = $(ev.currentTarget);
                        var id = parseInt(target.context.id, 10);
                        window.location.hash = '#/item/' + id;
                    },
                    gotoMain: function () {
                        successCallback("/main");
                    },
                    gotoMonster: function () {
                        successCallback("/monster");
                    },
                    gotoPlayer: function () {
                        successCallback("/PvP");
                    },
                    gotoHeal: function () {
                        successCallback("/construction");
                    },
                    gotoShop: function () {
                        successCallback("/shop");
                    }
                };
                return kendo.observable(viewModel);
            });
    }

    function getMonsterViewModel(successCallback) {
        return data.monsters.get()
            .then(function (list) {
                var index = getRand(0, list.length - 1);
                monster = list[index];
                console.log(monster);
                var viewModel = {
                    id: monster.id,
                    name: monster.name,
                    hp: monster.hp,
                    mp: monster.mp,
                    magicAttack: monster.magicAttack,
                    magicDefense: monster.magicDefense,
                    meleAttack: monster.meleAttack,
                    meleDefense: monster.meleDefense,
                    imageUrl: "/Images/monster.png",
                    gotoMain: function () {
                        successCallback("/main");
                    },
                    gotoMonster: function () {
                        successCallback("/monster");
                    },
                    gotoPlayer: function () {
                        successCallback("/PvP");
                    },
                    gotoHeal: function () {
                        successCallback("/construction");
                    },
                    gotoShop: function () {
                        successCallback("/shop");
                    },
                    startBattle: function () {
                        successCallback("/battleMonster/" + viewModel.id);
                    },
                };
                return kendo.observable(viewModel);
            });
    }

    function getItemViewModel(id, successCallback) {
        return data.items.getById(id)
            .then(function (item) {
                var viewModel = {
                    name: item.name,
                    description: item.description,
                    magicAttack: item.magicAttack,
                    meleAttack: item.meleAttack,
                    magicDefense: item.magicDefense,
                    meleDefense: item.meleDefense,
                    imageUrl: item.imageUrl,
                    itemCategory: item.itemCategory,
                    hp: item.hp,
                    mp: item.mp,
                    price: item.price,
                    buy: function (ev) {
                        var target = $(ev.currentTarget);
                        var gold = parseInt(localStorage.getItem("gold"), 10);
                        if (item.price > gold) {
                            alert("YOU No HAve EnOuGh MoNeY!");
                        }
                        else {
                            var updatedHero = hero();
                            if (updatedHero.items == undefined) {
                                updatedHero.items = [];
                                updatedHero.items.push(item);
                            }
                            else {
                                updatedHero.items.push(item);
                            }
                            gold -= item.price;
                            localStorage.setItem("gold", gold);
                            data.heros.update(updatedHero)
                            .then(function () {
                                localStorage.setItem("hero", JSON.stringify(updatedHero));
                                data.users.update(gold)
                                .then(function () {
                                    successCallback("/shop");
                                });
                            });
                        }
                    },
                    back: function () {
                        successCallback("/shop");
                    },
                    main: function () {
                        successCallback("/main");
                    },
                    monster: function () {
                        successCallback("/monster");
                    },
                    player: function () {
                        successCallback("/PvP");
                    },
                    heal: function () {
                        successCallback("/construction");
                    },
                    shop: function () {
                        successCallback("/shop");
                    }
                };

                return kendo.observable(viewModel);
            });
    }

    function getBattleMonsterViewModel(id, successCallback) {
        return data.monsters.getById(id)
            .then(function (item) {
                monster = item;
                player = hero();
                var viewModel = {
                    monsterFighter: {
                        id: item.id,
                        name: item.name,
                        hp: item.hp,
                        mp: item.mp,
                        maxHp: item.maxHp,
                        maxMp: item.maxMp,
                        magicAttack: item.magicAttack,
                        magicDefense: item.magicDefense,
                        meleAttack: item.meleAttack,
                        meleDefense: item.meleDefense,
                        imageUrl: "/Images/monster.png",
                    },
                    heroFighter: {
                        name: heroData.name,
                        hp: heroData.hp,
                        mp: heroData.mp,
                        maxHp: heroData.maxHp,
                        maxMp: heroData.maxMp,
                        magicAttack: heroData.magicAttack,
                        meleAttack: heroData.meleAttack,
                        magicDefense: heroData.magicDefense,
                        meleDefense: heroData.meleDefense,
                        imageUrl: "/Images/hero.png",
                    },
                    log: {

                    },
                    attackWithWeapon: function () {
                        player = hero();
                        monster.hp -= player.meleAttack - monster.meleDefense > 0 ? player.meleAttack - monster.meleDefense : 0;
                        if (monster.hp <= 0) {
                            alert("You won!");
                        }

                        var monsterAttack;
                        if (monster.meleAttack >= monster.magicAttack) {
                            monsterAttack = monster.meleAttack - player.meleDefense > 0 ? monster.meleAttack - player.meleDefense : 0;
                        }
                        else if (monster.mp < 10) {
                            monsterAttack = monster.meleAttack - player.meleDefense > 0 ? monster.meleAttack - player.meleDefense : 0;
                        }
                        else {
                            monsterAttack = monster.magicAttack - player.magicDefense > 0 ? monster.magicAttack - player.meleDefense : 0;
                            monster.mp -= 10;
                        }

                        player.hp -= monsterAttack;

                        successCallback(viewModel);
                    },
                    attackWithMagic: function () {
                        player = hero();
                        if (player.mp < 10) {
                            alert("You No HAvE EnouGh MaNa!");
                        }
                        else {
                            monster.hp -= player.magicAttack - monster.magicDefense > 0 ? player.magicAttack - monster.magicDefense : 0;
                            if (monster.hp <= 0) {
                                alert("You won!");
                            }

                            var monsterAttack;
                            if (monster.meleAttack >= monster.magicAttack) {
                                monsterAttack = monster.meleAttack - player.meleDefense > 0 ? monster.meleAttack - player.meleDefense : 0;
                            }
                            else if (monster.mp < 10) {
                                monsterAttack = monster.meleAttack - player.meleDefense > 0 ? monster.meleAttack - player.meleDefense : 0;
                            }
                            else {
                                monsterAttack = monster.magicAttack - player.magicDefense > 0 ? monster.magicAttack - player.meleDefense : 0;
                                monster.mp -= 10;
                            }

                            player.hp -= monsterAttack;

                            successCallback(viewModel);
                        }
                    },
                    finish: function () {
                        successCallback("/finish");
                    },
                    drinkPotion: function () {

                    },
                    //main: function () {
                    //    successCallback("/main");
                    //},
                    //monster: function () {
                    //    successCallback("/monster");
                    //},
                    //player: function () {
                    //    successCallback("/PvP");
                    //},
                    //heal: function () {
                    //    successCallback("/construction");
                    //},
                    //shop: function () {
                    //    successCallback("/shop");
                    //}
                };

                return kendo.observable(viewModel);
            });
    }

    function getPvPViewModel(successCallback) {
        return data.users.getAll()
            .then(function (allUsers) {
                var oponents = _.filter(allUsers, function (u) {
                    return u.username != localStorage.getItem('displayName');
                });
                var viewModel = {
                    players: oponents,
                    main: function () {
                        successCallback("/main");
                    },
                    monster: function () {
                        successCallback("/monster");
                    },
                    player: function () {
                        successCallback("/PvP");
                    },
                    heal: function () {
                        successCallback("/construction");
                    },
                    shop: function () {
                        successCallback("/shop");
                    }
                };
                return kendo.observable(viewModel);
            });
    }

    function getConstuctionViewModel(successCallback) {
        var viewModel = {
            main: function () {
                successCallback("/main");
            },
            monster: function () {
                successCallback("/monster");
            },
            player: function () {
                successCallback("/PvP");
            },
            heal: function () {
                successCallback("/construction");
            },
            shop: function () {
                successCallback("/shop");
            }
        };
        return kendo.observable(viewModel);
    }

    return {
        setPersister: function (persister) {
            data = persister;
        },
        getLoginVM: getLoginViewModel,
        getGameVM: getGameViewModel,
        getShopVM: getShopViewModel,
        getActionsVM: getActionsViewModel,
        getHeroVM: getHeroViewModel,
        getItemVM: getItemViewModel,
        getMonsterVM: getMonsterViewModel,
        getBattleMonsterVM: getBattleMonsterViewModel,
        getConstuctionVM: getConstuctionViewModel,
        getPvPVM: getPvPViewModel
    };
}());