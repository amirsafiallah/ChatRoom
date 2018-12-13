// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//global
var host = location.origin;
var socket;
var currentUser={
    userid:0,
    name:'System'
};

function initWebSocket() {
    var protocol = location.protocol === "https:" ? "wss:" : "ws:";
        var wsUri = protocol + "//" + window.location.host;
        socket = new WebSocket(wsUri);
        socket.onopen = e => {
            console.log("socket opened", e);
        };
        socket.onclose = function (e) {
            console.log("socket closed", e);
            setTimeout(function(){
                initWebSocket();
            }, 5000);
        };
        socket.onerror = function (e) {
            console.error(e.data);
        };
}
function initJquery(){
    $("#hide-list").click(function(){
        $('.online-list').hide();
    })
    $(".show-online-list").click(function(){
        $('.online-list').show();
    })
    $('.modal').modal({backdrop: 'static', keyboard: false});
}
function initAngularJs(){
    var app = angular.module('chatroom', []);
    app.controller('chat', function($scope,$http,$timeout) {
        initVariable();

        $http.get(host + "/api/message")
        .then((res) => {
            $scope.messages = $scope.messages.concat(res.data);
            scrollMessageList();
        }, (res) => {
            console.error(res);
        });

        $http.get(host + "/api/user")
        .then((res) => {
            res.data.forEach(function(e){
                $scope.users[e.id+''] = e.name;
            });
        }, (res) => {
            console.error(res);
        });

        $scope.login = function () {
            if (!$scope.fullname) return;
            $http.get(host + "/api/user/name/"+$scope.fullname)
            .then((res) => {
                currentUser = res.data;
                $('.modal').modal('hide');
            }, (res) => {
                console.error(res);
                $scope.userError = res.status;

            });
        }
        
        $scope.sendMessage = function () {
            socket.send(JSON.stringify({
                userid:currentUser.id,
                text:$scope.userMessage
            }));
            $scope.userMessage = '';
        }

        function letknow(id){
            if ($scope.users[id+'']) return;

            $http.get(host + "/api/user/"+id)
            .then((res) => {
                $scope.users[res.data.id] = res.data.name;
            }, (res) => {
                console.error(res);
            });
        }

        function scrollMessageList() {
            $timeout(function () {
                $(".message-container")
                    .animate({
                        scrollTop: $('.message-container').prop("scrollHeight")
                    }, 1000);
            });
        }

        function initVariable(){
            $scope.messages = [];
            $scope.users = {};
            socket.onmessage = function (e) {
                var msg = JSON.parse(e.data);
                $scope.$apply(function(){
                    $scope.messages.push(msg);
                    letknow(msg.userid);
                    scrollMessageList();
                });
            };
        }
    });

}
initAngularJs();
$(initJquery);
initWebSocket();