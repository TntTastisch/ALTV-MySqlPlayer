function sendData(type) {
    let loginUsername = $("#loginUsername").val();
    let loginPassword = $("#loginPassword").val();
    let registerUsername = $("#registerUsername").val();
    let registerPassword = $("#registerPassword").val();
    let registerPasswordRepeat = $("#registerPasswordRepeat").val();
    
    switch(type) {
        case 1:
            if(loginUsername.length < 4 || loginPassword.length < 4) {
            sendError(1, "Entries must be at least 4 characters long.");
            } else {
            if(!loginUsername.match(/([a-zA-Z]+)_([a-zA-Z]+)/)) {
                sendError(1, "The name must follow the format: Firstname_Lastname.");
                $(".login").css({
                    paddingBottom: "10px"
                });
                return;
            }
            //console.log(`mp.trigger("Client:tryLogin", ${loginUsername}, ${loginPassword});`)
            alt.emit("alttutorial:login", loginUsername, loginPassword);
            }
            break;

        case 2:
            if(registerUsername.length < 4 || registerPassword.length < 4) {
                sendError(2, "Entries must be at least 4 characters long.");
                } else {
                if(!registerUsername.match(/([a-zA-Z]+)_([a-zA-Z]+)/)) {
                    sendError(2, "The name must follow the format: Firstname_Lastname.");
                    $(".register").css({
                        paddingBottom: "4.5%"
                    });
                } else if(registerPassword != registerPasswordRepeat) {
                    sendError(2, "The passwords do not match.");
                } else {
                    //console.log(`mp.trigger("Client:tryRegister", ${registerUsername}, ${registerPassword});`)
                    alt.emit("alttutorial:register", registerUsername, registerPassword);
                }
            }
            break;
    }
}

function sendError(type, error) {
    switch(type) {
        case 1:
            $("#loginError").text(error);
            $("#loginError").show(500);
        break;

        case 2:
            $("#registerError").text(error);
            $("#registerError").show(500);
        break;
    }
}

function switchScreen(type) {
    switch(type) {
        case 1:
            $(".register").hide(500);
            $(".login").show(1000)
        break;

        case 2:
            $(".login").hide(500);
            $(".register").show(1000)
        break;
    }
}

alt.on('showError', (type, error) => sendError(type, error));